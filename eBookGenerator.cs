using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;

using System.IO;
using System.Xml;

namespace eBookGenerator
{
    public partial class eBookGenerator : Form
    {
        private string DATA_FILE_NAME = "Alliance";
        private string MODEL_DATA_FILE_NAME = "Models";
        private string LANGUAGE_DATA_FILE_NAME = "Languages.js";
        private string LOCALSTRINGS_DATA_FILE_NAME = "LocalStrings";
        private string CONFIG_DATA_FILE_NAME = "Config.js";
        private string DOC_DIR = @"\twain_32\tasks\assemblies\";

        private CDNames _cdNames = new CDNames();

        private string _cdName = "";
        private string _folderName = "";
        private string _dataFolderName = "";
        private string _imageName = "";

        public eBookGenerator()
        {
            InitializeComponent();
        }

        private void eBookGenerator_Load(object sender, EventArgs e)
        {
            foreach (CDName cd in _cdNames.GetList())
            {
                cboCDName.Items.Add(cd.LitCD);
            }

            cboCDName.SelectedIndex = 0;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                btnGo.Enabled = false;
                SetBookType();

                ProcessData();

                LogIt("");
                LogIt("");
                LogIt("DONE!");
                lblDone.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetBookType()
        {
            _cdName = cboCDName.SelectedItem.ToString();

            CDName cdName = _cdNames.GetByLitCD(_cdName);
            if (cdName != null)
            {
                _folderName = ConfigurationManager.AppSettings["CDRoot"] + cdName.OutputFolder;
                _dataFolderName = String.Format("{0}\\Data", _folderName);
                _imageName = cdName.ImageName;
            }
        }

        private void ProcessData()
        {
            LogIt("Reading tblEBookModelsToLit...");
            ModelsToLits modelToLits = new ModelsToLits(_cdName);
            LogIt(modelToLits.GetList().Count + " records found.");

            if (modelToLits.GetList().Count == 0)
            {
                MessageBox.Show("No Results Found.", "No Results");
            }

            CreateFolders();
            GenerateXMLData(modelToLits);
            CopyFiles();
            CopyDocs(_folderName, modelToLits);
        }

        private void CopyFiles()
        {
            CopyFile("als.css", _dataFolderName);
            CopyFile("jquery-1.7.2.min.js", _dataFolderName, "jquery.js");
            CopyFile("blank.pdf", _dataFolderName, "blank.pdf");

            CopyImage(_imageName, _dataFolderName);
            CopyImage("CD_Logo.png", _dataFolderName, "logo2.png");

            CopyAllFiles(_folderName);
        }

        private void CopyAllFiles(String destinationFolder)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"FilesForCD\CopyAllToCDRoot");
            FileInfo [] fileInfos = dirInfo.GetFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                String destinationFileName = fileInfo.Name;
                FileInfo tempFileInfo = fileInfo;
                tempFileInfo.CopyTo(destinationFolder + @"\" + destinationFileName);
                tempFileInfo = null;

                tempFileInfo = new FileInfo(destinationFolder + @"\" + destinationFileName);
                tempFileInfo.IsReadOnly = false;
                tempFileInfo = null;
            }
        }

        private void CopyFile(String sourceFileName, String destinationFolder)
        {
            CopyFile(sourceFileName, destinationFolder, sourceFileName);
        }

        private void CopyFile(String sourceFileName, String destinationFolder, String destinationFileName)
        {
            FileInfo fileInfo = new FileInfo(@"FilesForCD\" + sourceFileName);
            fileInfo.CopyTo(destinationFolder + @"\" + destinationFileName);
            fileInfo = null;

            fileInfo = new FileInfo(destinationFolder + @"\" + destinationFileName);
            fileInfo.IsReadOnly = false;
            fileInfo = null;
        }

        private void CopyImage(String sourceFileName, String destinationFolder)
        {
            CopyImage(sourceFileName, destinationFolder, sourceFileName);
        }

        private void CopyImage(String sourceFileName, String destinationFolder, String destinationFileName)
        {
            FileInfo fileInfo = new FileInfo(@"images\" + sourceFileName);
            fileInfo.CopyTo(destinationFolder + @"\" + destinationFileName);
            fileInfo = null;

            fileInfo = new FileInfo(destinationFolder + @"\" + destinationFileName);
            fileInfo.IsReadOnly = false;
            fileInfo = null;
        }

        private void CopyDocs(String folder, ModelsToLits modelToLits)
        {
            DirectoryInfo dir = new DirectoryInfo(_folderName + DOC_DIR);
            dir.Create();

            String rootDir = ConfigurationManager.AppSettings["DocRoot"];

            HashSet<String> notFoundList = new HashSet<string>();

            IList<String> partNumbers = modelToLits.GetList().Select(item => item.PartNo).Distinct().ToList();
            foreach (String partNumber in partNumbers)
            {
                String fileName = partNumber + ".pdf";
                FileInfo fileInfo = new FileInfo(rootDir + fileName);

                if (fileInfo.Exists)
                {
                    LogIt("Copying file " + fileInfo.Name + "...");
                    //FileInfo newFile = new FileInfo(folder + @"\docs\" + fileName);
                    fileInfo.CopyTo(folder + DOC_DIR + fileName, true);
                    fileInfo = null;

                    fileInfo = new FileInfo(folder + DOC_DIR + fileName);
                    fileInfo.IsReadOnly = false;
                    fileInfo = null;

                    WritePDFPage(partNumber);
                }
                else
                {
                    //LogIt("File " + fileInfo.Name + " not found.");
                    if (!notFoundList.Contains(fileInfo.Name))
                    {
                        notFoundList.Add(fileInfo.Name);
                    }
                }
            }

            foreach (String fName in notFoundList)
            {
                LogIt("File " + fName + " not found.");
                //System.Diagnostics.Debug.WriteLine(String.Format("Copy foo.pdf {0}", fName));
            }
       }

        private void LogIt(String item)
        {
            lstLogger.Items.Add(item);
            int visibleItems = lstLogger.ClientSize.Height / lstLogger.ItemHeight;
            lstLogger.TopIndex = Math.Max(lstLogger.Items.Count - visibleItems + 1, 0); 
            Application.DoEvents();
        }

        private void GenerateXMLData(ModelsToLits modelToLits)
        {
            Config config = new Config
            {
                ImageName = string.Format("Data/{0}", _imageName),
                LinkText = _imageName == "blank.gif" ? "&nbsp;" : "Please visit us at <a href='http://www.alliancelaundry.com'>www.alliancelaundry.com</a> for additional information."
            };
            config.WriteXMLFile(_folderName + @"\Data\" + CONFIG_DATA_FILE_NAME);

            Languages languages = new Languages(modelToLits);
            languages.WriteXMLFile(_folderName + @"\Data\" + LANGUAGE_DATA_FILE_NAME);

            foreach (Language language in languages.GetList())
            {
                LocalStrings localStrings = new LocalStrings();
                localStrings.WriteXMLFile(_folderName + @"\Data\" + LOCALSTRINGS_DATA_FILE_NAME, language);

                FileInfo dataFile = new FileInfo(_folderName + @"\Data\" + DATA_FILE_NAME + "_" + language.LanguageKey + ".js");
                StreamWriter sw = dataFile.CreateText();
                sw.Write(@"var _booksXML = ""<?xml version='1.0' encoding='utf-16' ?> ");
                sw.Write(@"<Books>");

                GenerateXMLModelData(modelToLits, language);

                IList<ModelsToLit> modelsToLitList = modelToLits.GetListByLanguageConverted(language);
                modelsToLitList = modelsToLitList.OrderBy(item => item.ModelNo).ThenByDescending(item => item.TypeOfLit).ThenByDescending(item => item.PartNo).ToList();

                foreach (ModelsToLit mtl in modelsToLitList)
                {
                    sw.Write(@"<Book>");

                    sw.Write(@"<ModelNo>" + mtl.ModelNo + @"</ModelNo>");
                    sw.Write(@"<ManufacturingNumber>" + mtl.ManufacturingNumber + @"</ManufacturingNumber>");
                    sw.Write(@"<ProdType>" + mtl.ProdType + @"</ProdType>");
                    sw.Write(@"<PartNo>" + mtl.PartNo + @"</PartNo>");
                    sw.Write(@"<TypeOfLit>" + mtl.TypeOfLit + @"</TypeOfLit>");
                    sw.Write(@"<Comments>" + mtl.Ecomments + @"</Comments>");
                    sw.Write(@"<LangCd>" + mtl.Lang + @"</LangCd>");

                    sw.Write(@"</Book>");
                }

                sw.Write(@"</Books>"";");

                sw.Flush();
                sw.Close();
                sw = null;
            }
        }

        private void GenerateXMLModelData(ModelsToLits modelToLits, Language language)
        {
            IList<ModelsToLit> models = modelToLits.GetModelList(language);

            FileInfo dataFile = new FileInfo(_folderName + @"\Data\" + MODEL_DATA_FILE_NAME + "_" + language.LanguageKey + ".js");
            StreamWriter sw = dataFile.CreateText();
            sw.Write(@"var _modelsXML=""<?xml version='1.0' encoding='utf-8' ?> ");
            sw.Write(@"<Books>");

            foreach (ModelsToLit mtl in models)
            {
                sw.Write(@"<Book>");

                sw.Write(@"<Type>1</Type>");
                sw.Write(@"<ManufacturingNumber>" + mtl.ManufacturingNumber + @"</ManufacturingNumber>");
                sw.Write(@"<ModelNo>" + mtl.ModelNo + @"</ModelNo>");
                sw.Write(@"<ProdType>" + mtl.ProdType + @"</ProdType>");

                sw.Write(@"</Book>");
            }

            models = modelToLits.GetManufacturerNumberList(language);
            foreach (ModelsToLit mtl in models.Where(x => String.IsNullOrEmpty(x.ManufacturingNumber) == false))
            {
                sw.Write(@"<Book>");

                sw.Write(@"<Type>2</Type>");
                sw.Write(@"<ManufacturingNumber></ManufacturingNumber>");
                sw.Write(@"<ModelNo>" + mtl.ManufacturingNumber + @"</ModelNo>");
                sw.Write(@"<ProdType></ProdType>");

                sw.Write(@"</Book>");
            }

            sw.WriteLine(@"</Books>"";");

            sw.Flush();
            sw.Close();
            sw = null;
        }

        private void CreateFolders()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_folderName);
            if (directoryInfo.Exists)
            {
                DialogResult dr = MessageBox.Show("The folder " + _folderName + " already exists.  Do you want to continue and overwrite the contents of this folder?", "Folder Already Exists", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    return;
                }
                DeleteFolder(directoryInfo);
            }

            directoryInfo.Create();

            DirectoryInfo dataDir = new DirectoryInfo(_dataFolderName);
            dataDir.Create();

        }

        private void DeleteFolder(DirectoryInfo directoryInfo)
        {
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                fileInfo.Delete();
            }
            Application.DoEvents();
            System.Threading.Thread.Sleep(1000);

            DirectoryInfo[] subDirectoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo subDirectoryInfo in subDirectoryInfos)
            {
                DeleteFolder(subDirectoryInfo);
            }

            directoryInfo.Delete();
        }

        private void WritePDFPage(String fileName)
        {
            FileInfo dataFile = new FileInfo(_folderName + @"/twain_32/tasks/assemblies/" + fileName + ".htm");
            StreamWriter sw = dataFile.CreateText();

            sw.WriteLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">");
            sw.WriteLine(@"<html xmlns=""http://www.w3.org/1999/xhtml"">");
            sw.WriteLine(@"");
            sw.WriteLine(@"    <head>");
            sw.WriteLine(@"      <!-- saved from url=(0014)about:internet -->");
            sw.WriteLine(@"    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>");
            sw.WriteLine(@"        <title>Alliance Laundry Systems</title>");
            sw.WriteLine(@"        <style type=""text/css"">");
            sw.WriteLine(@"            html { height: 100%; }");
            sw.WriteLine(@"            body { margin: 0; padding: 0; height: 100%; }");
            sw.WriteLine(@"            p { padding: 1em; }");
            sw.WriteLine(@"            object { display: block; }");
            sw.WriteLine(@"        </style>");
            sw.WriteLine(@"    </head>");
            sw.WriteLine(@"");
            sw.WriteLine(@"    <body>");
            sw.WriteLine(String.Format(@"      <object data=""{0}.pdf#toolbar=1&amp;navpanes=0&amp;scrollbar=1&amp;page=1&amp;view=FitH"" width=""100%"" type=""application/pdf"" height=""100%"">", fileName));
            sw.WriteLine(String.Format(@"        <param name=""src"" value=""{0}.pdf"">", fileName));
            sw.WriteLine(@"        <p>It appears you don't have a PDF plugin for this browser.</p>");
            sw.WriteLine(@"      </object>");
            sw.WriteLine(@"    </body>");
            sw.WriteLine(@"    ");
            sw.WriteLine(@"</html>");

            sw.Flush();
            sw.Close();
            sw = null;
        }
    }
}
