using ServiceLibrary.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceTypes.Constants;
using ServiceLibrary.Models.DataContracts;
using Patterns.Observer;

namespace ServiceLibrary.ConcreteService
{
    class CadWebServiceSimulation : ISolidWebService
    {
        Random randomExemplare = new Random();

        public void CreateDxf(int[] filesId)
        {
            MessageObserver.Instance.SetMessage($"Created {filesId.Length - 1} dxf files");
        }

        public void CreatePdf(int[] filesId)
        {
            MessageObserver.Instance.SetMessage($"Created {filesId.Length - 1} pdf files");
        }

        public void CreateSpigot(SpigotType_e type, int width, int height)
        {
            MessageObserver.Instance.SetMessage($"Created Spigot [type:{type}, width:{width}, height: {height}]");
        }

        public void ExportPartDataToXml(TransmittableSpecification[] specification)
        {
            MessageObserver.Instance.SetMessage($"Got specification; It's Length-1: {specification.Length - 1}");
        }
       
        public TransmittableTaskData[] GetActiveTasksData( )
        {
            TransmittableTaskData[] transmittableTaskData = new TransmittableTaskData[randomExemplare.Next(3, 7)];

            for (byte i = (byte)( transmittableTaskData.Length- 1) ; i != 0; i--)
            {
                transmittableTaskData[i] = new TransmittableTaskData
                {
                    UserId = randomExemplare.Next(10, 5000),
                    Designation = $"test designation task: {i}",
                    Status = randomExemplare.Next(1, 4),
                    TaskId = randomExemplare.Next(1, 10),       //TODO refactoring
                    Type = randomExemplare.Next(1, 4)
                };
            }

            MessageObserver.Instance.SetMessage($"Got request GetActiveTasksData: Returns TaskData array");
            return transmittableTaskData;
        }


        private string GenerateText( )
        {
            string source = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder resultBuilder = new StringBuilder( );

            for (byte i = 0; i < 2; i++)
            {
                resultBuilder.Append(source[randomExemplare.Next(0, source.Length - 1)]);
                resultBuilder.Append(source[randomExemplare.Next(0, source.Length - 1)]);
                resultBuilder.Append("-");
                resultBuilder.Append(randomExemplare.Next(50 - 200));
                resultBuilder.Append("-");
            }
            return resultBuilder.ToString( );
        }

        public TransmittableTaskData[] GetComplitedTasksData( )
        {
            TransmittableTaskData[] transmittableTaskData = new TransmittableTaskData[randomExemplare.Next(3, 7)];

            for (byte i =(byte) (transmittableTaskData.Length - 1); i != 0; i--)
            {
                transmittableTaskData[i] = new TransmittableTaskData
                {
                    UserId = randomExemplare.Next(10, 5000),
                    Designation = $"test designation task: {i}",
                    Status = randomExemplare.Next(1, 4),
                    TaskId = randomExemplare.Next(1, 10),       //TODO refactoring
                    Type = randomExemplare.Next(1, 4)
                };
            }

            MessageObserver.Instance.SetMessage($"Got request GetComplitedTasksData: Returns TaskData array");
            return transmittableTaskData;
        }
        string[] configigurations = new string[] { "03", "00", "TDWS", "04", "01", "05"};
        public string[] GetConfigigurations(string filePath)
        {


            return configigurations;
        }

        public string GetPathSelectFile(TransmittableFileModel dataSolidModel)
        {
            return @"P:\test\path";
        }

        public TransmittableSpecification[] GetSpecifications(string filePath, string configuration)
        {
            TransmittableSpecification[] specification = new TransmittableSpecification[randomExemplare.Next(10, 50)];

            for (byte i = (byte)(specification.Length - 1); i != 0; i--)
            {
                specification[i].Bend = randomExemplare.Next(70, 120).ToString( );
                specification[i].CodeMaterial = randomExemplare.Next(800, 1200).ToString( );
                specification[i].Configuration = configigurations[randomExemplare.Next(0, configigurations.Length - 1)];
                specification[i].Count = randomExemplare.Next(1, 10);
                specification[i].Description = GenerateText( );
                specification[i].ERPCode = randomExemplare.Next(1000, 30000).ToString( );
                specification[i].Type = randomExemplare.Next(1000, 5000).ToString( );
                specification[i].Version = randomExemplare.Next(1, 24).ToString( );
                specification[i].Weight = randomExemplare.Next(800, 1200).ToString( );
                specification[i].WorkpieceX = randomExemplare.Next(540, 3000).ToString( );
                specification[i].WorkpieceY = randomExemplare.Next(540, 3000).ToString( );
                specification[i].FileName = GenerateText( );
                specification[i].IDPDM = specification[i].ERPCode = randomExemplare.Next(1000, 30000).ToString( );
                specification[i].isDxf = (randomExemplare.Next(0, 1) == 0 ? false : true).ToString( );
                specification[i].Level = randomExemplare.Next(1, 5);
                specification[i].PaintX = randomExemplare.Next(540, 3000).ToString( );
                specification[i].PaintY = randomExemplare.Next(540, 3000).ToString( );
                specification[i].PaintZ = randomExemplare.Next(540, 3000).ToString( );
                specification[i].Partition = GenerateText( );
                specification[i].PartNumber = randomExemplare.Next(540, 3000).ToString( );
                specification[i].SummMaterial = randomExemplare.Next(540, 3000).ToString( );
                specification[i].SurfaceArea = randomExemplare.Next(1000, 3000).ToString( );
                specification[i].Thickness = randomExemplare.Next(30, 300).ToString( );
            }
            return specification;
        }

        public bool isServiceWork( )
        {
            return true;
        }

        public TransmittableFileModel[] Search(string nameSegment)
        {
            TransmittableFileModel[] TransmittableFileModels = new TransmittableFileModel[randomExemplare.Next(0, 30)];
            byte i = (byte)(TransmittableFileModels.Length - 1);
            for (; i != 0; i--)
            {
                TransmittableFileModels[i] = new TransmittableFileModel( )
                {
                    CurrentVersion = randomExemplare.Next(1, 24),
                    Id = randomExemplare.Next(1000, 30000),
                    FileName = GenerateText( ),
                    FolderPath = $@"P:\test\path\{GenerateText( )}",
                    FolderId = randomExemplare.Next(1000, 30000),
                    Path = $@"P:\test\path\{GenerateText( )}\{GenerateText( )}.SLDPRT"
                };
            }
            return TransmittableFileModels;
        }

        public TransmittableFile SelectFile(TransmittableFileModel dataSolidModel)
        {
            string testStr = GenerateText( );
            byte[] byteCode = new byte[testStr.Length];
            for (byte i = 0; i < byteCode.Length; i++)
            {
                byteCode[i] = (byte)testStr[i];
            }
            return new TransmittableFile(dataSolidModel.FileName, byteCode.Length, byteCode);
        }
    }
}
