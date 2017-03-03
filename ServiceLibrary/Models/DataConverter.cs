
using PdmSolidWorksLibrary;
using PdmSolidWorksLibrary.Models;
using ServiceLibrary.Models.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskSystemLibrary;

namespace ServiceLibrary.Models
{
    public abstract class DataConverter
    {
        public static FileModelPdm ToFileModelPdm(TransmittableFileModel fileModel)
        {
            return new FileModelPdm
            {
                IDPdm = fileModel.Id,
                CurrentVersion = fileModel.CurrentVersion,
                FileName = fileModel.FileName,
                FolderId = fileModel.FolderId,
                FolderPath = fileModel.FolderPath,
                Path = fileModel.Path
            };
        }


        public static TransmittableFileModel ToTransferFileModel(FileModelPdm fileModel)
        {
            return new TransmittableFileModel
            {
                Id = fileModel.IDPdm,
                CurrentVersion = fileModel.CurrentVersion,
                FileName = fileModel.FileName,
                FolderId = fileModel.FolderId,
                FolderPath = fileModel.FolderPath,
                Path = fileModel.Path
            };
        }



        public static IEnumerable<FileModelPdm> ToFileModelPdm(IEnumerable<TransmittableFileModel> fileModels)
        {
            List<FileModelPdm> fileModelPdmList = new List<FileModelPdm>();
            foreach (var eachFileModel in fileModels)
            {
                fileModelPdmList.Add(ToFileModelPdm(eachFileModel));
            }
            return null;
        }


        public static IEnumerable<TransmittableFileModel> ToTransferFileModel(IEnumerable<FileModelPdm> fileModels)
        {
            List<TransmittableFileModel> transferFileModelList = new List<TransmittableFileModel>();
            foreach (var eachFileModel in fileModels)
            {
                transferFileModelList.Add(ToTransferFileModel(eachFileModel));
            }
            return transferFileModelList;
        }


        public static TransmittableSpecification[] GetSpecification(string filePath, string configuration)
        {
            try
            {
                var parts = DataBaseDomian.SwPlusRepository.Instance.Parts;
                var bomShell = SolidWorksPdmAdapter.Instance.GetBomShell(filePath, configuration);
                //Console.WriteLine(parts.Count());
                //Console.WriteLine(bomShell.Count());
                var specifications = from eachBom in bomShell
                                     join eachPart in parts on new { id = (int)eachBom.IdPdm, ver = (int)eachBom.LastVesion, conf = eachBom.Configuration }
                                     equals new { id = (int)eachPart.IDPDM, ver = (int)eachPart.Version, conf = eachPart.ConfigurationName }
                                     into Spec_s
                                     from spec in Spec_s.DefaultIfEmpty()
                                     select new TransmittableSpecification
                                     {
                                         Description = eachBom.Description,
                                         Count = (int) eachBom.Count  ,
                                         Weight = eachBom.Weight,
                                         Partition = eachBom.Partition,
                                         PartNumber = eachBom.PartNumber,
                                         ERPCode = eachBom.ErpCode,
                                         SummMaterial = eachBom.SummMaterial,
                                         Configuration = eachBom.Configuration,
                                         IDPDM = eachBom.IdPdm.ToString(),
                                         CodeMaterial = eachBom.CodeMaterial,
                                         Type = eachBom.FileType,
                                         Level = (int) eachBom.Level,

                                         Version = (spec == null ? string.Empty : spec.Version.ToString()),
                                         Bend = spec == null ? string.Empty : spec.Bend.ToString(),
                                         PaintX = spec == null ? string.Empty : spec.PaintX.ToString(),
                                         PaintY = spec == null ? string.Empty : spec.PaintY.ToString(),
                                         PaintZ = spec == null ? string.Empty : spec.PaintZ.ToString(),
                                         WorkpieceX = spec == null ? string.Empty : spec.WorkpieceX.ToString(),
                                         WorkpieceY = spec == null ? string.Empty : spec.WorkpieceY.ToString(),
                                         SurfaceArea = spec == null ? string.Empty : spec.SurfaceArea.ToString(),
                                         // isDxf = System.Convert.ToInt32(spec.DXF) == 1 ? true : false
                                         isDxf = spec != null && spec.DXF == "1"   ? "true" : "false"

                                        , FileName = eachBom.FileName

                                     };
                Patterns.Observer.MessageObserver.Instance.SetMessage("Got specification", Patterns.Observer.MessageType.Success);
                return specifications.ToArray();
            }
            catch (Exception ex)
            {
                Patterns.Observer.MessageObserver.Instance.SetMessage("Failed get specification " + ex, Patterns.Observer.MessageType.Error);
                throw ex;
            }
        }

        public static TransmittableTaskData ToTaskDataTransmittable( TaskData taskData )
        {
            return new TransmittableTaskData
            {
                TaskId = taskData.TaskId,
                UserId = taskData.UserId,
                Status = taskData.Status,
                Type = taskData.TaskType
            };
        }
        public static IEnumerable<TransmittableTaskData >ToTaskDataTransmittable(IEnumerable< TaskData> taskDataList)
        {
            List<TransmittableTaskData> TransmittableTaskDataList = new List<TransmittableTaskData>();
            foreach (var eachtaskData in taskDataList)
            {
                TransmittableTaskDataList.Add(ToTaskDataTransmittable(eachtaskData));

            }

            return TransmittableTaskDataList;
        }
    }
}
