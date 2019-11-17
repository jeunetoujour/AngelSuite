using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;

public static class AutoUpdate
{
    private static string m_RemotePath = string.Empty;
    private static string m_UpdateFileName = "AutoUpdate.txt";
    private static string m_ErrorMessage = "AutoUpdate experienced a technical fault.";

    // <File Name>;<Version>   [| comments    ]
    // <File Name>[;<Version>] [| comments    ]  
    // <File Name>[;?]         [| comments    ]
    // <File Name>[;delete]    [| comments    ]
    // ...
    // Blank lines and comments are ignored
    // The first line should be the current program/version
    // from the second line to the end the second parameter is optional
    // if the second parameter is not specified the file is updated.
    // if the version is specified the update checks the version
    // if the second parameter is an interrogation mark (?) the update checks if the 
    // file alredy exists and "don't" upgrade if exists.
    // if the second parameter is "delete" the system try to delete the file
    // A pipe (|) starts a line comment

    // Function Return Value
    // True means that the program needs to exit because: the autoupdate did the update
    // or there was an error during the update
    // False - nothing was done

    public enum UpdateStatus
    {
        UpdatedSuccessfully,
        NoConnectionToServer,
        ErrorAtServer,
        ErrorInAutoUpdate,
        UpdateMismatch,
        NothingToUpdate
    }

    public static UpdateStatus UpdateFiles()
    {
        return UpdateFiles(string.Empty);
    }

    public static UpdateStatus UpdateFiles(string RemotePath)
    {
        return UpdateFiles(RemotePath, true);
    }

    public static string Log
    {
        get
        {
            return string.Empty;
        }
        set
        {
            try
            {
                File.AppendAllText(Application.StartupPath + "\\AutoUpdate.log", DateTime.Now.ToString() + " :: " + value + "\r\n");
            }
            catch { }
        }
    }

    public static UpdateStatus UpdateFiles(string RemotePath, bool bWantDebug)
    {
        if (string.IsNullOrEmpty(RemotePath)) RemotePath = m_RemotePath; else m_RemotePath = RemotePath;

        UpdateStatus Ret = UpdateStatus.NothingToUpdate;
        string AssemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        string ToDeleteExtension = "._del";
        string RemoteUri = RemotePath + "/";
        WebClient MyWebClient = new WebClient();
        string s;
        string Contents;
        FileVersionInfo fv;

        UpdateProgress progressCue = new UpdateProgress();
        progressCue.Progress = 0;
        progressCue.Show();

        if (bWantDebug) Log = "AutoUpdater started";
        if (bWantDebug) Log = "... our assembly name is: " + AssemblyName;
        if (bWantDebug) Log = "... our remote URI is:    " + RemoteUri + UpdateFileName;

        try
        {
            progressCue.Status = "Cleaning Up Old Files";

            // Perform clean-up (delete *._del files left over from prior update)
            try
            {
                string[] sFilesToDelete = System.IO.Directory.GetFiles(Application.StartupPath, "*" + ToDeleteExtension, SearchOption.TopDirectoryOnly);
                if (sFilesToDelete.Length > 0) s = sFilesToDelete[0]; else s = null;
                while (!string.IsNullOrEmpty(s))
                {
                    if (bWantDebug) Log = "... found to delete: " + s;
                    File.Delete(s);
                    sFilesToDelete = System.IO.Directory.GetFiles(Application.StartupPath, "*" + ToDeleteExtension, SearchOption.TopDirectoryOnly);
                    if (sFilesToDelete.Length > 0) s = sFilesToDelete[0]; else s = null;
                }
            }
            catch { }

            progressCue.Progress = 5;

            // Download update file (usually AutoUpdate.txt)
            progressCue.Status = "Searching For Updates";
            if (bWantDebug) Log = "... downloading " + UpdateFileName;
            try { Contents = MyWebClient.DownloadString(RemoteUri + UpdateFileName); }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    if (ex.Message.Contains("(404)"))
                    {
                        if (bWantDebug) Log = "... error: 404 at server";
                        progressCue.Hide();
                        return UpdateStatus.ErrorAtServer;
                    }
                }

                if (bWantDebug) Log = "... error: other exception, see details:";
                if (bWantDebug) Log = GetAllErrorDetails(ex);
                progressCue.Hide();
                return UpdateStatus.NoConnectionToServer;
            }


            // Parse update file (usually AutoUpdate.txt)
            string[] FileList;
            try
            {
                if (bWantDebug) Log = "... shuffling list of files from " + UpdateFileName;

                FileList = Contents.Replace("\r", string.Empty).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Remove all comments and blank lines
                Contents = string.Empty;
                foreach (string F_iteration in FileList)
                {
                    string F = F_iteration;
                    if (F.IndexOf("|") > 0)
                        F = F.Substring(0, F.IndexOf("|"));

                    if (!String.IsNullOrEmpty(F.Trim()))
                    {
                        if (!string.IsNullOrEmpty(Contents))
                            Contents += "\n";
                        Contents += F.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                if (bWantDebug) Log = "... error: exception during file shuffle, see details:";
                if (bWantDebug) Log = GetAllErrorDetails(ex);
                progressCue.Hide();
                return UpdateStatus.ErrorInAutoUpdate;
            }


            // Rebuild the file list
            string[] Info = new string[] { string.Empty, string.Empty };
            try
            {
                FileList = Contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                Info = FileList[0].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                if (bWantDebug) Log = "... error: exception during file list rebuild, see details:";
                if (bWantDebug) Log = GetAllErrorDetails(ex);
                progressCue.Hide();
                return UpdateStatus.ErrorInAutoUpdate;
            }


            int iCountUpdatedFiles = 0;


            // If the name is correct and it is a new version...
            if (bWantDebug) Log = "... this looks like auto-update information for: " + Info[0];
            if (bWantDebug) Log = "... local version is:                            " + GetVersion(Application.ProductVersion);
            if (Info.Length > 1) { if (bWantDebug) Log = "... the server's latest version is:              " + GetVersion(Info[1]); }

            if (Application.StartupPath.ToLower() + "\\" + Info[0].ToLower() == Application.ExecutablePath.ToLower())
            {
                if (bWantDebug) Log = "... proceeding to check for updated files";

                // Process files in the list
                int iFileNum = 0;
                int iFileMax = FileList.Length;

                foreach (string F in FileList)
                {
                    iFileNum++;

                    progressCue.Progress = ((((iFileNum - 1) * 100) / iFileMax) * 90 / 100) + 5;

                    Info = F.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    bool isToDelete = false;
                    bool isToUpgrade = false;
                    string TempFileName = Application.StartupPath + "\\" + DateTime.Now.TimeOfDay.TotalMilliseconds;
                    string FileName = Application.StartupPath + "\\" + Info[0].Trim();
                    bool FileExists = File.Exists(FileName);

                    if (bWantDebug) Log = "....... filename:       " + Info[0].Trim();
                    if (Info.Length > 1) { if (bWantDebug) Log = "....... version/method: " + Info[1].Trim(); }
                    else { if (bWantDebug) Log = "....... version/method: always upgrade"; }

                    if (Info.Length == 1)
                    {
                        // If just the file was given without any parameters
                        // (upgrade the existing file unconditionally)
                        isToUpgrade = true;
                        isToDelete = FileExists;
                        if (bWantDebug) Log = "........... unconditional upgrade";
                    }
                    else if (Info[1].Trim() == "delete")
                    {
                        // If the second parameter is:  delete
                        // (delete the existing file)
                        isToDelete = FileExists;
                        if (bWantDebug) Log = "........... delete request if file exists";
                        if (bWantDebug) Log = "........... file exists = " + FileExists.ToString();
                    }
                    else if (Info[1].Trim() == "?")
                    {
                        // If the second parameter is:  ?
                        // (download if missing, but do not upgrade existing versions)
                        isToUpgrade = !FileExists;
                        if (bWantDebug) Log = "........... upgrade if file doesn't exist";
                        if (bWantDebug) Log = "........... file exists = " + FileExists.ToString();
                    }
                    else if (FileExists)
                    {
                        // If the file exists AND a version # was specified
                        // (compare the version information and upgrade if necessary)
                        fv = FileVersionInfo.GetVersionInfo(FileName);
                        isToUpgrade = String.Compare(GetVersion(Info[1].Trim()), GetVersion(fv.FileMajorPart + "." + fv.FileMinorPart + "." + fv.FileBuildPart + "." + fv.FilePrivatePart)) > 0;
                        isToDelete = isToUpgrade;
                        if (bWantDebug) Log = "........... upgrade if new version is available";
                        if (bWantDebug) Log = "........... local version:    " + GetVersion(fv.FileMajorPart + "." + fv.FileMinorPart + "." + fv.FileBuildPart + "." + fv.FilePrivatePart);
                        if (bWantDebug) Log = "........... server's version: " + GetVersion(Info[1].Trim());
                    }
                    else
                    {
                        // Otherwise, the file didn't already exist -- we don't care what version the
                        // server has, we're going to download it anyway.
                        isToUpgrade = true;
                        if (bWantDebug) Log = "........... download server version (local didn't exist)";
                    }


                    // Download the new version
                    if (isToUpgrade)
                    {
                        if (bWantDebug) Log = "........... attempt to download latest version";
                        progressCue.Status = "Downloading " + Info[0];
                        MyWebClient.DownloadFile(RemoteUri + Info[0], TempFileName);
                    }

                    if (isToUpgrade && ((FileName.Trim().ToLower().EndsWith(".exe")) || (FileName.Trim().ToLower().EndsWith(".dll"))) && (Info.Length == 1))
                        iCountUpdatedFiles++;
                    else if (isToUpgrade && ((FileName.Trim().ToLower().EndsWith(".exe")) || (FileName.Trim().ToLower().EndsWith(".dll"))) && (Info[1].Trim() != "?"))
                    {
                        if (bWantDebug) Log = "............... latest version is an executable/library, check version mismatch";

                        // Test the new EXE/DLL's version to make sure it really did update correctly
                        fv = FileVersionInfo.GetVersionInfo(TempFileName);
                        string sNewVersion = GetVersion(fv.FileMajorPart + "." + fv.FileMinorPart + "." + fv.FileBuildPart + "." + fv.FilePrivatePart);
                        if (bWantDebug) Log = "................... version we downloaded is " + sNewVersion;
                        if (GetVersion(Info[1].Trim()) != sNewVersion)
                        {
                            if (bWantDebug) Log = "................... this does not match, deleting it and aborting";
                            File.Delete(TempFileName);
                            progressCue.Hide();
                            return UpdateStatus.UpdateMismatch;
                        }
                        else
                            iCountUpdatedFiles++;

                        try
                        {
                            string FileName2 = Application.StartupPath + "\\" + Info[0].ToLower().Replace(".exe", ".pdb").Replace(".dll", ".pdb").Trim();
                            File.Delete(FileName2);
                            if (bWantDebug) Log = "............... also downloading any available PDB";
                        }
                        catch
                        {
                        }

                    }
                    else if (isToUpgrade)
                        iCountUpdatedFiles++;


                    // Rename existing file (it will be deleted on a subsequent launch)
                    if (isToDelete)
                    {
                        if (bWantDebug) Log = "........... rename existing file to delete it";
                        File.Move(FileName, TempFileName + ToDeleteExtension);
                    }

                    // Rename the downloaded file to the real name
                    if (isToUpgrade)
                    {
                        if (bWantDebug) Log = "........... rename downloaded file so it's used on the next launch";
                        File.Move(TempFileName, FileName);
                    }
                }

                if (iCountUpdatedFiles > 0)
                {
                    if (bWantDebug) Log = "... return update success";
                    Ret = UpdateStatus.UpdatedSuccessfully;
                }
                else
                    if (bWantDebug) Log = "... the product hasn't been updated, closing out...";
            }
            else
                if (bWantDebug) Log = "... the product hasn't been updated, closing out...";
        }
        catch (Exception ex)
        {
            Ret = UpdateStatus.ErrorInAutoUpdate;
            if (bWantDebug) Log = "... error: exception somewhere, see details:";
            if (bWantDebug) Log = GetAllErrorDetails(ex);
        }
        progressCue.Progress = 100;
        progressCue.Hide();
        return Ret;
    }


    public static UpdateStatus CheckForUpdates(string RemotePath, bool bWantDebug)
    {
        if (string.IsNullOrEmpty(RemotePath)) RemotePath = m_RemotePath; else m_RemotePath = RemotePath;

        UpdateStatus Ret = UpdateStatus.NothingToUpdate;
        string AssemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        string RemoteUri = RemotePath + AssemblyName + "/";
        WebClient MyWebClient = new WebClient();
        string Contents;
        FileVersionInfo fv;

        if (bWantDebug) Log = "AutoUpdater started";
        if (bWantDebug) Log = "... our assembly name is: " + AssemblyName;
        if (bWantDebug) Log = "... our remote URI is:    " + RemoteUri + UpdateFileName;

        try
        {
            // Download update file (usually AutoUpdate.txt)
            if (bWantDebug) Log = "... downloading " + UpdateFileName;
            try { Contents = MyWebClient.DownloadString(RemoteUri + UpdateFileName); }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    if (ex.Message.Contains("(404)"))
                    {
                        if (bWantDebug) Log = "... error: 404 at server";
                        return UpdateStatus.ErrorAtServer;
                    }
                }

                if (bWantDebug) Log = "... error: other exception, see details:";
                if (bWantDebug) Log = GetAllErrorDetails(ex);
                return UpdateStatus.NoConnectionToServer;
            }


            // Parse update file (usually AutoUpdate.txt)
            string[] FileList;
            try
            {
                if (bWantDebug) Log = "... shuffling list of files from " + UpdateFileName;

                FileList = Contents.Replace("\r", string.Empty).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Remove all comments and blank lines
                Contents = string.Empty;
                foreach (string F_iteration in FileList)
                {
                    string F = F_iteration;
                    if (F.IndexOf("|") > 0)
                        F = F.Substring(0, F.IndexOf("|"));

                    if (!String.IsNullOrEmpty(F.Trim()))
                    {
                        if (!string.IsNullOrEmpty(Contents))
                            Contents += "\n";
                        Contents += F.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                if (bWantDebug) Log = "... error: exception during file shuffle, see details:";
                if (bWantDebug) Log = GetAllErrorDetails(ex);
                return UpdateStatus.ErrorInAutoUpdate;
            }


            // Rebuild the file list
            string[] Info = new string[] { string.Empty, string.Empty };
            try
            {
                FileList = Contents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                Info = FileList[0].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                if (bWantDebug) Log = "... error: exception during file list rebuild, see details:";
                if (bWantDebug) Log = GetAllErrorDetails(ex);
                return UpdateStatus.ErrorInAutoUpdate;
            }


            int iCountFilesToUpdate = 0;


            // If the name is correct and it is a new version...
            if (bWantDebug) Log = "... this looks like auto-update information for: " + Info[0];
            if (bWantDebug) Log = "... local version is:                            " + GetVersion(Application.ProductVersion);
            if (Info.Length > 1) { if (bWantDebug) Log = "... the server's latest version is:              " + GetVersion(Info[1]); }
            
            if (Application.StartupPath.ToLower() + "\\" + Info[0].ToLower() == Application.ExecutablePath.ToLower())
            {
                if (bWantDebug) Log = "... proceeding to check for updated files";

                // Process files in the list
                int iFileNum = 0;
                int iFileMax = FileList.Length;

                foreach (string F in FileList)
                {
                    iFileNum++;

                    Info = F.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    bool isToDelete = false;
                    bool isToUpgrade = false;
                    string TempFileName = Application.StartupPath + "\\" + DateTime.Now.TimeOfDay.TotalMilliseconds;
                    string FileName = Application.StartupPath + "\\" + Info[0].Trim();
                    bool FileExists = File.Exists(FileName);

                    if (bWantDebug) Log = "....... filename:       " + Info[0].Trim();
                    if (Info.Length > 1) { if (bWantDebug) Log = "....... version/method: " + Info[1].Trim(); }
                    else { if (bWantDebug) Log = "....... version/method: always upgrade"; }

                    if (Info.Length == 1)
                    {
                        // If just the file was given without any parameters
                        // (upgrade the existing file unconditionally)
                        isToUpgrade = true;
                        isToDelete = FileExists;
                        if (bWantDebug) Log = "........... unconditional upgrade";
                    }
                    else if (Info[1].Trim() == "delete")
                    {
                        // If the second parameter is:  delete
                        // (delete the existing file)
                        isToDelete = FileExists;
                        if (bWantDebug) Log = "........... delete request if file exists";
                        if (bWantDebug) Log = "........... file exists = " + FileExists.ToString();
                    }
                    else if (Info[1].Trim() == "?")
                    {
                        // If the second parameter is:  ?
                        // (download if missing, but do not upgrade existing versions)
                        isToUpgrade = !FileExists;
                        if (bWantDebug) Log = "........... upgrade if file doesn't exist";
                        if (bWantDebug) Log = "........... file exists = " + FileExists.ToString();
                    }
                    else if (FileExists)
                    {
                        // If the file exists AND a version # was specified
                        // (compare the version information and upgrade if necessary)
                        fv = FileVersionInfo.GetVersionInfo(FileName);
                        isToUpgrade = String.Compare(GetVersion(Info[1].Trim()), GetVersion(fv.FileMajorPart + "." + fv.FileMinorPart + "." + fv.FileBuildPart + "." + fv.FilePrivatePart)) > 0;
                        isToDelete = isToUpgrade;
                        if (bWantDebug) Log = "........... upgrade if new version is available";
                        if (bWantDebug) Log = "........... local version:    " + GetVersion(fv.FileMajorPart + "." + fv.FileMinorPart + "." + fv.FileBuildPart + "." + fv.FilePrivatePart);
                        if (bWantDebug) Log = "........... server's version: " + GetVersion(Info[1].Trim());
                    }
                    else
                    {
                        // Otherwise, the file didn't already exist -- we don't care what version the
                        // server has, we're going to download it anyway.
                        isToUpgrade = true;
                        if (bWantDebug) Log = "........... download server version (local didn't exist)";
                    }


                    // Download the new version
                    if (isToUpgrade)
                    {
                        if (bWantDebug) Log = "........... new version detected (this is check-only mode)";
                        iCountFilesToUpdate++;
                    }
                }

                if (iCountFilesToUpdate > 0)
                {
                    if (bWantDebug) Log = "... return update success";
                    Ret = UpdateStatus.UpdatedSuccessfully;
                }
                else
                    if (bWantDebug) Log = "... the product hasn't been updated, closing out...";
            }
            else
                if (bWantDebug) Log = "... the product hasn't been updated, closing out...";
        }
        catch (Exception ex)
        {
            Ret = UpdateStatus.ErrorInAutoUpdate;
            if (bWantDebug) Log = "... error: exception somewhere, see details:";
            if (bWantDebug) Log = GetAllErrorDetails(ex);
        }
        return Ret;
    }

    public static string GetAllErrorDetails(Exception e)
    {
        String sTXTError = string.Empty;
        System.Collections.IEnumerator ienum;

        if (Exception.Equals(e.GetBaseException(), e) == false)
        {
            sTXTError += GetAllErrorDetails(e.GetBaseException());
            sTXTError += "\r\n";
            sTXTError += "\r\n";
            sTXTError += "-----------------------------------------------------------------\r\n";
        }

        if (e.Message != null) sTXTError += "[MESSAGE]   " + e.Message + "\r\n";
        if (e.InnerException != null)
            sTXTError += "[INNER]     \r\n" + e.InnerException.Message + "\r\n";
        sTXTError += "-----------------------------------------------------------------\r\n";
        sTXTError += "[TARGET]\r\n";
        sTXTError += "Name:       " + e.TargetSite.Name + "()\r\n";
        sTXTError += "Module:     " + e.TargetSite.Module.Name + "\r\n";
        sTXTError += "Attributes: " + e.TargetSite.Attributes.ToString() + "\r\n";
        sTXTError += "-----------------------------------------------------------------\r\n";

        if (e.Data != null)
        {
            if (e.Data.Count > 0)
            {
                sTXTError += "[DATA]\r\n";
                ienum = e.Data.GetEnumerator();
                while (ienum.MoveNext())
                    sTXTError += ienum.Current.ToString() + "\r\n";
                sTXTError += "-----------------------------------------------------------------\r\n";
            }
        }

        sTXTError += "[STACK TRACE]\r\n";
        sTXTError += e.StackTrace.Replace(" in c:\\", " in: C:\\").Replace(".cs:line ", ".cs (line ") + ")\r\n";
        sTXTError += "-----------------------------------------------------------------\r\n";

        return sTXTError;
    }

    public static string RemotePath
    {
        get { return m_RemotePath; }
        set { m_RemotePath = value; }
    }

    public static string UpdateFileName
    {
        get { return m_UpdateFileName; }
        set { m_UpdateFileName = value; }
    }

    public static string ErrorMessage
    {
        get { return m_ErrorMessage; }
        set { m_ErrorMessage = value; }
    }

    private static string GetVersion(string Version)
    {
        string[] x = Version.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
        return string.Format("{0:00000}{1:00000}{2:00000}{3:00000}", int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2]), int.Parse(x[3]));
    }
}
