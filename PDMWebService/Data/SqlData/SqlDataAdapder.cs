using PDM_WebService.WcfServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDMWebService.Data.SqlData
{
    public class SqlDataAdapder : Singleton.AbstractSingeton<SqlDataAdapder>
    {
        private SwPlusDbLinqORMDataContext swPlusContext { get; set; }
        private SqlDataAdapder() : base()
        {
            this.swPlusContext = new SwPlusDbLinqORMDataContext();
        }

        public IEnumerable<Specification> GetSpecifications(BomShell[] boms)
        {

            try
            {



                var specifications = from eachBom in boms
                                     join eachPart in swPlusContext.View_Parts on new { id = (int)eachBom.IdPdm, ver = (int)eachBom.LastVesion, conf = eachBom.Configuration }
                                     equals new { id = (int)eachPart.IDPDM, ver = (int)eachPart.Version, conf = eachPart.ConfigurationName }
                                     into Spec_s
                                     from spec in Spec_s.DefaultIfEmpty<View_Part>()
                                     select new Specification
                                     {
                                         Name = eachBom.Name,
                                         Count = eachBom.Count.ToString(),
                                         Weight = eachBom.Weight,
                                         Partition = eachBom.Partition,
                                         Designation = eachBom.Designation,
                                         ERPCode = eachBom.ErpCode,
                                         SummMaterial = eachBom.SummMaterial,
                                         Configuration = eachBom.Configuration,
                                         IDPDM = eachBom.IdPdm.ToString(),
                                         CodeMaterial = eachBom.CodeMaterial,

                                         Version = (spec == null ? string.Empty : spec.Version.ToString()),
                                         Bend = spec == null ? string.Empty : spec.Bend.ToString(),
                                         PaintX = spec == null ? string.Empty : spec.PaintX.ToString(),
                                         PaintY = spec == null ? string.Empty : spec.PaintY.ToString(),
                                         PaintZ = spec == null ? string.Empty : spec.PaintZ.ToString(),
                                         WorkpieceX = spec == null ? string.Empty : spec.WorkpieceX.ToString(),
                                         WorkpieceY = spec == null ? string.Empty : spec.WorkpieceY.ToString(),
                                         SurfaceArea = spec == null ? string.Empty : spec.SurfaceArea.ToString()
                                     };


                var specToArray = specifications.ToArray();


                for (int i = 0; i < specToArray.Length; i++)
                {
                    int idPdm;
                    int version;

                    if (Int32.TryParse(specToArray[i].IDPDM, out idPdm) && Int32.TryParse(specToArray[i].Version, out version))
                    {
                        specToArray[i].isDxf = this.CheckDXF(idPdm, specToArray[i].Configuration, version);
                    }
                    else
                    {
                        specToArray[i].isDxf = false;
                    }

                    //specToArray[i].isDxf = this.CheckDXF
                    //    (
                    //    Int32.Parse(specToArray[i].IDPDM),
                    //    specToArray[i].Configuration,
                    //    Int32.Parse(specToArray[i].Version)
                    //    );
                }

                return specToArray;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

        }


        public bool CheckDXF(int idPdm, string configuration, int version)
        {
            int checkResult = this.swPlusContext.DXFCheck(idPdm, configuration, version);
            if (checkResult == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void UploadDxf(byte[] dxf, int idPdm, string configuration, int version)
        {
            try
            {
             //   swPlusContext.ExportDXF(new System.Data.Linq.Binary(dxf), idPdm, configuration, version);          
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }
    }
}
