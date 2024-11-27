using BusinessLogicLayer.Dto.Query;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services.Common;
using DataAccessLayer.Common;
using DataAccessLayer.Entites;
using DataAccessLayer.Repository;
using Services.Helper;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

namespace BusinessLogicLayer.Services
{

    public class SiesaServices
    {
        private readonly PlainBuilderManagerServices _plainBuilderManagerServices;
        private readonly SiesaPlainConnector _siesaPlainConnector;
        private readonly InputParameterRepository _inputParameterRepository;
        private readonly SettingStorageRepository _settingStorageRepository;
        private readonly PlainBuilderManagerRepository _plainBuilderManagerRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly ConfigurationRepository _configurationRepository;
        private readonly ProcessDataStockRepository _processDataBatchRepository;
        private readonly InterfaceReceptorRepository _interfaceReceptorRepository;
        private readonly ProcessConfOrdenFabricacionRepository _processConfOrdenFabricacionRepository;
        public SiesaServices()
        {
            _plainBuilderManagerServices = new PlainBuilderManagerServices();
            _siesaPlainConnector = new SiesaPlainConnector();
            _inputParameterRepository = new InputParameterRepository();
            _settingStorageRepository = new SettingStorageRepository();
            _plainBuilderManagerRepository = new PlainBuilderManagerRepository();
            _companyRepository = new CompanyRepository();
            _configurationRepository = new ConfigurationRepository();
            _processDataBatchRepository = new ProcessDataStockRepository();
            _interfaceReceptorRepository = new InterfaceReceptorRepository();
            _processConfOrdenFabricacionRepository = new ProcessConfOrdenFabricacionRepository();
        }
        public ServiceResponse<EscribirPlanoOutput> PlanoEntradadeAlmacen(RecibirConfRecepcionDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();

            var parameters = new InputParameter();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }
                var f350_fecha = data.ConfRecepcion.Fecfin;
                var docto = data.ConfRecepcion.Pedext.Split('-');
                var docto2 = data.ConfRecepcion.Proext.Split('-');
                var f350_nota = data.ConfRecepcion.Nument;
                var albara = data.ConfRecepcion.Albara;

                string f350_id_tercero;
                string f350_id_co;
                string f420_id_tipo_docto;
                string f420_consec_docto;
                string f470_id_lote = string.Empty;

                var f350_id_tipo_docto = string.Empty;
                var f350_consec_docto = string.Empty;
                try
                {
                    f350_id_tercero = docto2[0];
                    f350_id_co = docto[0];
                    f420_id_tipo_docto = docto[1];
                    f420_consec_docto = docto[2];
                }
                catch (Exception ex)
                {
                    throw new Exception("Error a leer esta propiedad Pedext");
                }
                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var settingStorage = configuration.FirstOrDefault(t => t.Name == "QDM.Inlog.Query.GetAllEntradaAlmacenCabecera" && t.Id_Cia == data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                var query = settingStorage.Value;
                query = string.Format(query,
                         f350_id_co,
                         f420_id_tipo_docto,
                         f420_consec_docto,
                         data.ConfRecepcion.Proext,
                         data.SiesaCode,
                         string.Empty);

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaAlmacenCabeceraDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerSr.Status)
                {
                    throw new Exception(plainBuilderManagerSr.Errors.FirstOrDefault().ErrorDetail);
                }
                var plainBuilderManager = plainBuilderManagerSr.Data.FirstOrDefault();
                var f451_id_sucursal_prov = plainBuilderManager.id_sucursal_prov;
                var f451_id_tercero_comprador = plainBuilderManager.id_tercero_comprador;
                var f451_num_docto_referencia = albara;
                var f451_ind_consignacion = plainBuilderManager.ind_consignacion;
                var f350_ind_estado = plainBuilderManager.ind_estado;
                var ind_importacion = plainBuilderManager.ind_importacion;
                var f41850_id_co_docto = plainBuilderManager.id_co_importaciones;
                var f41850_id_tipo_docto = plainBuilderManager.id_tipo_docto_importaciones;
                var f41850_consec_docto = plainBuilderManager.consec_docto_importaciones;

                ////TODO:todas las validaciones que tenga que hacer
                //var generalParams = inputParameter
                //                        .Where(t => t.CompanyId == data.CompanyId
                //                                && t.PlainKeyName == "QDM.Siesa.PlainWs.GetEntradaAlmacen"
                //                                && t.ParameterOption.KeyName == Constants.PARAMETEROPTION)
                //                        .FirstOrDefault();

                //if (generalParams == null)
                //{
                //    throw new Exception("los parametros generales no se encuentran configurados");
                //}

                int consecutivo = 0;
                var docSection = new SectionData();
                if (ind_importacion == "0")
                {
                    parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWs.GetEntradaAlmacen" && t.CO == f350_id_co);
                    if (parameters == null)
                    {
                        throw new Exception("los parametros generales no se encuentran configurados");
                    }
                    parameters.Consecutive = parameters.Consecutive.Value + 1;
                    //TODO:todas la consulta a base de datos
                    f350_id_tipo_docto = parameters.DocumentType;
                    //sumarle 1 y actualizar el parametro
                    //var f350_consec_docto = parameters.Consecutive.Value.ToString();
                    f350_consec_docto = parameters.Consecutive.Value.ToString();

                    docSection = new SectionData
                    {
                        KeyName = "EntradaDocumentos V.3",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f350_id_co",                Value=f350_id_co},
                            new FieldData { KeyName = "f350_id_tipo_docto",        Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "f350_consec_docto",         Value=f350_consec_docto},
                            new FieldData { KeyName = "f350_fecha",                Value=f350_fecha},
                            new FieldData { KeyName = "f350_id_tercero",           Value=f350_id_tercero},
                            new FieldData { KeyName = "f451_id_sucursal_prov",     Value=f451_id_sucursal_prov},
                            new FieldData { KeyName = "f451_id_tercero_comprador", Value=f451_id_tercero_comprador},
                            new FieldData { KeyName = "f451_num_docto_referencia", Value=f451_num_docto_referencia},
                            new FieldData { KeyName = "f451_ind_consignacion",     Value=f451_ind_consignacion},
                            new FieldData { KeyName = "f420_id_tipo_docto",        Value=f420_id_tipo_docto},
                            new FieldData { KeyName = "f420_consec_docto",         Value=f420_consec_docto},
                            new FieldData { KeyName = "f420_id_co_docto",          Value=f350_id_co},
                            new FieldData { KeyName = "f350_notas",                Value=f350_nota},
                            new FieldData { KeyName = "f350_ind_estado",           Value=f350_ind_estado}
                        }
                    };
                }
                else
                {
                    parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWs.GetEntradaAlmacenImpor" && t.CO == f350_id_co);
                    if (parameters == null)
                    {
                        throw new Exception("los parametros generales no se encuentran configurados");
                    }
                    parameters.Consecutive = parameters.Consecutive.Value + 1;
                    //TODO:todas la consulta a base de datos
                    f350_id_tipo_docto = parameters.DocumentType;
                    //sumarle 1 y actualizar el parametro
                    //var f350_consec_docto = parameters.Consecutive.Value.ToString();
                    f350_consec_docto = parameters.Consecutive.Value.ToString();

                    docSection = new SectionData
                    {
                        KeyName = "EntradaDocumentos V.11",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f350_id_co",                Value=f350_id_co},
                            new FieldData { KeyName = "f350_id_tipo_docto",        Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "f350_consec_docto",         Value=f350_consec_docto},
                            new FieldData { KeyName = "f350_fecha",                Value=f350_fecha},
                            new FieldData { KeyName = "f350_id_tercero",           Value=f350_id_tercero},
                            new FieldData { KeyName = "f350_ind_estado",           Value=f350_ind_estado},
                            new FieldData { KeyName = "f350_notas",                Value=f350_nota},
                            new FieldData { KeyName = "f451_id_sucursal_prov",     Value=f451_id_sucursal_prov},
                            new FieldData { KeyName = "f451_num_docto_referencia", Value=f451_num_docto_referencia},
                            new FieldData { KeyName = "f41850_id_co_docto",        Value=f41850_id_co_docto},
                            new FieldData { KeyName = "f41850_id_tipo_docto",      Value=f41850_id_tipo_docto},
                            new FieldData { KeyName = "f41850_consec_docto",       Value=f41850_consec_docto}
                        }
                    };
                }


                int f421_nro_registro = data.ConfLineaRecep.ConfLineaRecepcion.Count;
                settingStorage = _settingStorageRepository
                                        .GetKeyAndCompany("QDM.Inlog.Query.GetAllEntradaAlmacenDetalle", data.CompanyId);
                if (inputParameter == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                query = settingStorage.Value;
                query = string.Format(query,
                         f350_id_co,
                         f420_id_tipo_docto,
                         f420_consec_docto,
                         data.ConfRecepcion.Proext,
                         data.SiesaCode,
                         string.Empty);

                compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaAlmacenCabeceraDetalleDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }

                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.ToList();
                var checkDetail = new ServiceResponse();

                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                if (!interfaceReceptorSr.Status)
                {
                    isLote = false;
                }
                isLote = interfaceReceptorSr.Data.IsStatus;
                var lines = data.ConfLineaRecep.ConfLineaRecepcion
                           .GroupBy(x => isLote
                               ? new { x.Pedext, x.LoteFa, x.Articu, x.Codlin, x.Varia1, x.Varia2 }
                               : new { x.Pedext, LoteFa = (string)null, x.Articu, x.Codlin, x.Varia1, x.Varia2 })
                           .Select(g => new
                           {
                               Pedext = g.Key.Pedext,
                               Articu = g.Key.Articu,
                               Codlin = g.Key.Codlin,
                               LoteFa = g.Key.LoteFa,
                               Varia1 = g.Key.Varia1,
                               Varia2 = g.Key.Varia2,
                               Items = g.ToList(),
                               TotalCantco = g.Sum(t => int.Parse(t.Cantco)),
                           })
                           .ToList();

                foreach (var item in lines)
                {
                    var artTemp = plainBuilderManagerDetail.Where(t => t.id_item == item.Articu && t.rowidright.ToString() == item.Codlin);

                    if (!artTemp.Any())
                    {
                        checkDetail.AddError(new Exception($"Para este articulo {item.Articu} no se  encontre ninguna realacion  con Codlin {item.Codlin}, por favor validar..."));
                        continue;
                    }
                    var art = artTemp.FirstOrDefault();
                    //TODO:todas la consulta a base de datos
                    var f470_id_bodega = art.id_bodega;
                    var f470_id_ubicacion_aux = art.id_ubicacion_aux;
                    var f470_id_unidad_medida = art.id_unidad_medida;
                    var f421_fecha_entrega = art.fecha_entrega.ToString("yyyyMMdd");
                    var factor = art.factor;

                    var f470_cant_base = (item.TotalCantco / factor);
                    var f470_rowid = art.rowid;
                    //TODO: check it have decimal f470_cant_base if have decimal then item.TotalCantco item.id_unidad_inventario else f470_cant_base
                    //bool hasDecimals = item.TotalCantco % factor != 0;

                    //if (hasDecimals)
                    //{
                    //    f470_cant_base = item.TotalCantco.ToString();
                    //    f470_id_unidad_medida = art.id_unidad_inventario;
                    //}
                    if (f470_cant_base == 0)
                    {
                        //checkDetail.AddError(new Exception($"La cantidad de este articulo {item.Articu} es la siguiente {f470_cant_base.ToString()} fue calculada y no es valida"));
                        continue;
                    }
                    var lote = string.Empty;
                    if (isLote)
                    {
                        if (!string.IsNullOrEmpty(item.LoteFa))
                        {
                            lote = item.LoteFa.Length > 15
                            ? item.LoteFa.Substring(item.LoteFa.Length - 15, 15)
                            : item.LoteFa;
                        }
                    }

                    f421_nro_registro++;
                    if (ind_importacion == "0")
                    {
                        movementsSection.Add(new SectionData
                        {
                            KeyName = "EntradaMovimientos V.6",
                            Fields = new List<FieldData>
                            {
                                new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                                new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                                new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                                new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                                new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                                new FieldData { KeyName = "f470_id_bodega",          Value=f470_id_bodega},
                                new FieldData { KeyName = "f470_id_ubicacion_aux",   Value=f470_id_ubicacion_aux},
                                new FieldData { KeyName = "f470_id_unidad_medida",   Value=f470_id_unidad_medida},
                                new FieldData { KeyName = "f421_fecha_entrega",      Value=f421_fecha_entrega},
                                new FieldData { KeyName = "f470_cant_base",          Value=f470_cant_base.ToString()},
                                new FieldData { KeyName = "f470_id_item",            Value=item.Articu},
                                new FieldData { KeyName = "f470_rowid",              Value=f470_rowid},
                                new FieldData { KeyName = "f470_id_lote",            Value=lote},
                                new FieldData { KeyName = "f470_id_ext1_detalle",    Value=!item.Varia1.Equals("0") ? item.Varia1:string.Empty},
                                new FieldData { KeyName = "f470_id_ext2_detalle",    Value=!item.Varia2.Equals("0") ? item.Varia2:string.Empty},
                            }
                        });
                    }
                    else
                    {
                        movementsSection.Add(new SectionData
                        {
                            KeyName = "EntradaMovimientos V.25",
                            Fields = new List<FieldData>
                            {
                                new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                                new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                                new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                                new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                                new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                                new FieldData { KeyName = "f470_id_bodega",          Value=f470_id_bodega},
                                new FieldData { KeyName = "f470_id_ubicacion_aux",   Value=f470_id_ubicacion_aux},
                                new FieldData { KeyName = "f470_id_lote",            Value=lote},
                                new FieldData { KeyName = "f470_id_unidad_medida",   Value=f470_id_unidad_medida},
                                new FieldData { KeyName = "f470_cant_base",          Value=f470_cant_base.ToString()},
                                new FieldData { KeyName = "f470_id_item",            Value=item.Articu},
                                new FieldData { KeyName = "f470_id_ext1_detalle",    Value=!item.Varia1.Equals("0") ? item.Varia1:string.Empty},
                                new FieldData { KeyName = "f470_id_ext2_detalle",    Value=!item.Varia2.Equals("0") ? item.Varia2:string.Empty},
                                new FieldData { KeyName = "f41851_id_co_docto_oc",   Value=f350_id_co},
                                new FieldData { KeyName = "f41851_id_tipo_docto_oc", Value=f420_id_tipo_docto},
                                new FieldData { KeyName = "f41851_consec_docto_oc",  Value=f420_consec_docto},
                                new FieldData { KeyName = "f41851_rowid_movto_oc",   Value=f470_rowid}
                            }
                        });
                    }
                }
                if (!checkDetail.Status)
                {
                    string msg = string.Join(", ", checkDetail.Errors.Select(p => $"{p.ErrorMessage}"));
                    throw new Exception(msg);
                }

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.AddRange(movementsSection);

                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO ENTRADA DE ALMACEN= INLOG_{consecutivo}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                //respuesta.Data.Mensaje = !siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription) ? siesaResponse.MessageDescription : siesaResponse.ResponseMessage;

                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                _inputParameterRepository.Update(parameters);
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoDevolucionesDeClientes(CustomerReturnConfirmationDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            string f350_id_tipo_docto = string.Empty;
            string f350_consec_docto = string.Empty;
            string fecha = string.Empty;
            string f350_id_co = string.Empty;
            string f430_id_tipo_docto = string.Empty;
            string f430_consec_docto = string.Empty;
            InputParameter parameters = new InputParameter();
            List<Configuration> configuration = new List<Configuration>();
            DateTime dato_fecha = new DateTime();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }
                var docto = data.ConfDevolucionCliente.Numdoc.Split('-');
                try
                {
                    f350_id_co = docto[0];
                    f430_id_tipo_docto = docto[1];
                    f430_consec_docto = docto[2];
                }
                catch (Exception ex)
                {
                    throw new Exception("Error a leer esta propiedad NUMDOC");
                }
                configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                isLote = interfaceReceptorSr.Status;

                parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWs.GetDevolucionCliente" && t.CO == f350_id_co);

                //TODO:todas las validaciones que tenga que hacer
                var generalParams = inputParameter
                                        .Where(t => t.CompanyId == data.CompanyId
                                                && t.PlainKeyName == parameters.PlainKeyName
                                                && t.ParameterOption.KeyName == Constants.PARAMETEROPTION)
                                        .FirstOrDefault();

                if (generalParams == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                parameters.Consecutive = parameters.Consecutive.Value + 1;
                //TODO:todas la consulta a base de datos
                f350_id_tipo_docto = parameters.DocumentType;
                f350_consec_docto = parameters.Consecutive.Value.ToString();
                fecha = DateTime.Now.ToString("yyyyMMdd");

                //Consulta de detalle devoluciones
                var settingStorage = _settingStorageRepository
                                       .GetKeyAndCompany("QDM.Inlog.Query.GetAllDevolucionClienteDetalle", data.CompanyId);
                if (inputParameter == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var query = settingStorage.Value;
                query = string.Format(query,
                         f350_id_co,
                         f430_id_tipo_docto,
                         f430_consec_docto,
                         data.SiesaCode,
                         string.Empty);

                var companyRepository = _companyRepository.Get(data.CompanyId);
                if (companyRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerDetailListSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaMovimientoDevolucionClienteDetalleDto>(query,
                                                        companyRepository.Server,
                                                        companyRepository.DataBase,
                                                        companyRepository.UserName,
                                                        companyRepository.Password);
                if (!plainBuilderManagerDetailListSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailListSr.Errors.FirstOrDefault().ErrorDetail);
                }

                var plainBuilderManagerDetailList = plainBuilderManagerDetailListSr.Data.ToList();

                if (plainBuilderManagerDetailList.Count() > 0)
                {
                    f430_id_tipo_docto = plainBuilderManagerDetailList.FirstOrDefault().id_tipo_docto;
                    f430_consec_docto = plainBuilderManagerDetailList.FirstOrDefault().consec_docto;
                }

                //DevolucionDocumentos V.3
                var docSection = new SectionData
                {
                    KeyName = "DevolucionDocumentos V.3",
                    Fields = new List<FieldData>
                    {
                        new FieldData { KeyName = "F_CIA",             Value=data.SiesaCode.ToString()},
                        new FieldData { KeyName = "F350_ID_CO",        Value=f350_id_co},
                        new FieldData { KeyName = "F350_ID_TIPO_DOCTO",Value=f350_id_tipo_docto},
                        new FieldData { KeyName = "F350_CONSEC_DOCTO", Value=f350_consec_docto},
                        new FieldData { KeyName = "F350_FECHA",        Value=fecha},
                        new FieldData { KeyName = "F430_ID_TIPO_DOCTO",Value=f430_id_tipo_docto},
                        new FieldData { KeyName = "F430_CONSEC_DOCTO", Value=f430_consec_docto},
                    }
                };


                var movementsSection = new SectionData
                {
                    KeyName = "Movimiento CxC V.1",
                    Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "F350_ID_CO",              Value=f350_id_co},
                            new FieldData { KeyName = "F350_ID_TIPO_DOCTO",      Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "F350_CONSEC_DOCTO",       Value=f350_consec_docto},
                            new FieldData { KeyName = "F353_ID_TIPO_DOCTO_CRUCE",Value=f430_id_tipo_docto},
                            new FieldData { KeyName = "F353_CONSEC_DOCTO_CRUCE", Value=f430_consec_docto},
                            new FieldData { KeyName = "F353_FECHA_VCTO",         Value=fecha},
                            new FieldData { KeyName = "F353_FECHA_DSCTO_PP",     Value=fecha},
                        }
                };


                //int f421_nro_registro = data.ConfLineaDevolucionCliente.LineasDevolucionCliente.Count;
                int f421_nro_registro = 1;

                List<SectionData> discountsSection = new List<SectionData>();
                foreach (var item in data.ConfLineaDevolucionCliente.LineasDevolucionCliente)
                {
                    int idItem = int.Parse(item.Articu);
                    var plainBuilderManagerDetail = plainBuilderManagerDetailList.FirstOrDefault(x => x.id_item == idItem);

                    //DevolucionMovimientos V.6
                    var f470_id_bodega = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_bodega_ap;
                    var f470_id_ubicacion_aux = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_ubicacion_aux;
                    var f470_id_motivo = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_motivo;
                    var f470_id_co_movto = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_co_movto;
                    var f470_id_un_movto = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_un_movto;
                    var f470_id_unidad_medida = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_unidad_medida;
                    var f470_rowid_movto = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.rowid_movto;
                    var f470_ind_obsequio = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.ind_obsequio;
                    var f470_ind_impto_asumido = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.ind_impto_asumido;

                    var lote = string.Empty;

                    if (isLote)
                    {
                        if (!string.IsNullOrEmpty(item.Lotefa))
                        {
                            lote = item.Lotefa.Length > 15
                           ? item.Lotefa.Substring(item.Lotefa.Length - 15, 15)
                           : item.Lotefa;
                        }
                    }

                    if (!string.IsNullOrEmpty(item.Canrea) && int.Parse(item.Canrea) > 0)
                    {
                        discountsSection.Add(new SectionData
                        {
                            KeyName = "DevolucionMovimientos V.6",
                            Fields = new List<FieldData>
                            {
                                new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                                new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                                new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                                new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                                new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                                new FieldData { KeyName = "f470_id_item",            Value=item.Articu},
                                new FieldData { KeyName = "f470_id_bodega",          Value=f470_id_bodega},
                                new FieldData { KeyName = "f470_id_ubicacion_aux",   Value=f470_id_ubicacion_aux},
                                new FieldData { KeyName = "f470_id_motivo",          Value=f470_id_motivo},
                                new FieldData { KeyName = "f470_id_co_movto",        Value=f470_id_co_movto},
                                new FieldData { KeyName = "f470_id_un_movto",        Value=f470_id_un_movto},
                                new FieldData { KeyName = "f470_id_unidad_medida",   Value=f470_id_unidad_medida},
                                new FieldData { KeyName = "f470_rowid_movto",        Value=f470_rowid_movto},
                                new FieldData { KeyName = "f470_cant_base",          Value=item.Canrea},
                                new FieldData { KeyName = "f470_id_causal_devol",    Value=item.Causad},
                                new FieldData { KeyName = "f470_ind_obsequio",       Value=f470_ind_obsequio},
                                new FieldData { KeyName = "f470_ind_impto_asumido",  Value=f470_ind_impto_asumido},
                                new FieldData { KeyName = "f470_id_lote",            Value=lote}
                            }
                        });
                        f421_nro_registro++;
                    }

                    if (!string.IsNullOrEmpty(item.Cantna) && int.Parse(item.Cantna) > 0)
                    {
                        f470_id_bodega = plainBuilderManagerDetail == null ? string.Empty : plainBuilderManagerDetail.id_bodega_np;

                        discountsSection.Add(new SectionData
                        {
                            KeyName = "DevolucionMovimientos V.6",
                            Fields = new List<FieldData>
                            {
                                new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                                new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                                new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                                new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                                new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                                new FieldData { KeyName = "f470_id_item",            Value=item.Articu},
                                new FieldData { KeyName = "f470_id_bodega",          Value=f470_id_bodega},
                                new FieldData { KeyName = "f470_id_motivo",          Value=f470_id_motivo},
                                new FieldData { KeyName = "f470_id_co_movto",        Value=f470_id_co_movto},
                                new FieldData { KeyName = "f470_id_un_movto",        Value=f470_id_un_movto},
                                new FieldData { KeyName = "f470_id_unidad_medida",   Value=f470_id_unidad_medida},
                                new FieldData { KeyName = "f470_rowid_movto",        Value=f470_rowid_movto},
                                new FieldData { KeyName = "f470_cant_base",          Value=item.Cantna},
                                new FieldData { KeyName = "f470_id_causal_devol",    Value=item.Causad},
                                new FieldData { KeyName = "f470_ind_obsequio",       Value=f470_ind_obsequio},
                                new FieldData { KeyName = "f470_ind_impto_asumido",  Value=f470_ind_impto_asumido},
                                new FieldData { KeyName = "f470_id_lote",            Value=lote}
                            }
                        });
                        f421_nro_registro++;
                    }
                }
                dato_fecha = plainBuilderManagerDetailList.Count() > 0 ? plainBuilderManagerDetailList.FirstOrDefault().dato_fecha : DateTime.Now;
                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.Add(movementsSection);
                plainOC.Sections.AddRange(discountsSection);

                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO DEVOLUCIONES DE CLIENTES= INLOG_{parameters.Consecutive.Value}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                _inputParameterRepository.Update(parameters);


            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            try
            {
                var f753_dato_fecha_hora = dato_fecha.ToString("yyyyMMdd");
                var resp = PlanoEntidades(data, f350_id_tipo_docto, f350_consec_docto, f753_dato_fecha_hora, f350_id_co, f430_id_tipo_docto, f430_consec_docto, parameters, configuration);
                if (!resp.Status)
                {
                    new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, $"Error: PlanoNotaCredito {resp.Errors.FirstOrDefault().ErrorMessage}", Guid.NewGuid().ToString(), OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR);
                }
            }
            catch (Exception ex)
            {
                new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, $"Error: PlanoNotaCredito {ex.ToString()}", Guid.NewGuid().ToString(), OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR);
            }


            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoTransferenciaInventario(ChangeLogicalSituationDto data, Regulation regulation = Regulation.NotRegulatization, string source = "ChangeLogicalSituation")
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();
            List<ProcessDataStock> _processDataStock = new List<ProcessDataStock>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var configurationData = _configurationRepository.Get("QDM.Inlog.WebService.SendingStockBatch", data.CompanyId);
                if (configurationData == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var f350_id_co = data.ConfCambioSituacionLogica.Almace;
                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }

                var parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWsTransferenciasInv" && t.CO == f350_id_co);
                if (parameters == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var settingStorage = _settingStorageRepository
                                      .GetKeyAndCompany("QDM.Inlog.Query.GetAllCambioSituacionLogica", data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                parameters.Consecutive = parameters.Consecutive.Value + 1;

                var f350_id_tipo_docto = parameters.DocumentType;
                var f350_consec_docto = parameters.Consecutive.Value.ToString();
                var f350_fecha = DateTime.Now.ToString("yyyyMMdd");

                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                isLote = interfaceReceptorSr.Status;

                var query = settingStorage.Value;
                query = string.Format(query,
                         data.ConfCambioSituacionLogica.Stviej.ToUpper(),
                         data.ConfCambioSituacionLogica.Stnuev.ToUpper(),
                         data.ConfCambioSituacionLogica.Articu,
                         data.SiesaCode,
                         string.Empty);

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<CambioSituacionLogicaDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }
                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.FirstOrDefault();

                int f421_nro_registro = 1;
                var f470_id_unidad_medida = plainBuilderManagerDetail.id_unidad_medida;
                var f450_id_bodega_salida = plainBuilderManagerDetail.id_bodega_salida;
                var f450_id_bodega_entrada = plainBuilderManagerDetail.id_bodega_entrada;

                if (string.IsNullOrEmpty(f450_id_bodega_salida) || string.IsNullOrEmpty(f450_id_bodega_entrada))
                {
                    throw new Exception($"Para estos valores Stviej:({data.ConfCambioSituacionLogica.Stviej})-Stnuev:({data.ConfCambioSituacionLogica.Stnuev}) no aplica esta interface");
                }

                var lote = string.Empty;
                if (isLote)
                {
                    if (!string.IsNullOrEmpty(data.ConfCambioSituacionLogica.Lotefa))
                    {
                        lote = data.ConfCambioSituacionLogica.Lotefa.Length > 15
                       ? data.ConfCambioSituacionLogica.Lotefa.Substring(data.ConfCambioSituacionLogica.Lotefa.Length - 15, 15)
                       : data.ConfCambioSituacionLogica.Lotefa;
                    }
                }

                var docSection = new SectionData
                {
                    KeyName = "documentosV2",
                    Fields = new List<FieldData>
                {
                    new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                    new FieldData { KeyName = "f350_id_co",                Value=f350_id_co},
                    new FieldData { KeyName = "f350_id_tipo_docto",        Value=f350_id_tipo_docto},
                    new FieldData { KeyName = "f350_consec_docto",         Value=f350_consec_docto},
                    new FieldData { KeyName = "f350_fecha",                Value=f350_fecha},
                    new FieldData { KeyName = "f450_id_bodega_salida",     Value=f450_id_bodega_salida},
                    new FieldData { KeyName = "f450_id_bodega_entrada",    Value=f450_id_bodega_entrada},
                }
                };

                movementsSection.Add(new SectionData
                {
                    KeyName = "movimientosV5",
                    Fields = new List<FieldData>
                {
                    new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                    new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                    new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                    new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                    new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                    new FieldData { KeyName = "f470_id_bodega",          Value=f450_id_bodega_salida},
                    new FieldData { KeyName = "f470_id_co_movto",        Value=f350_id_co},
                    new FieldData { KeyName = "f470_id_unidad_medida",   Value=f470_id_unidad_medida},
                    new FieldData { KeyName = "f470_cant_base",          Value=data.ConfCambioSituacionLogica.Cantid},
                    new FieldData { KeyName = "f470_id_item",            Value=data.ConfCambioSituacionLogica.Articu},
                    new FieldData { KeyName = "f470_id_lote",            Value=lote},
                }
                });

                _processDataStock.Add(new ProcessDataStock
                {
                    f470_id_co = f350_id_co,
                    f470_id_bodega = f450_id_bodega_salida,
                    f450_id_bodega_salida = f450_id_bodega_salida,
                    f450_id_bodega_entrada = f450_id_bodega_entrada,
                    f470_id_co_movto = f350_id_co,
                    f470_id_unidad_medida = f470_id_unidad_medida,
                    f470_cant_base = data.ConfCambioSituacionLogica.Cantid,
                    f470_id_item = data.ConfCambioSituacionLogica.Articu,
                    KeyName = "QDM.Siesa.PlainWsTransferenciasInv",
                    PlainKeyName = parameters.PlainKeyName.Trim(),
                    SiesaCode = data.SiesaCode.ToString(),
                    Status = StatusProcess.PENDING,
                    CreatedDate = DateTime.Now,
                    KeyHeader = "documentosV2",
                    KeyItems = "movimientosV5",
                    Source = source,
                    CompanyId = data.CompanyId
                });

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.AddRange(movementsSection);
                if (configurationData.Value.Equals("1"))
                {
                    var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                    try
                    {
                        var fileGenerate = newPlain.File;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                    }

                    string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                    string responseMessage = $"PLANO TRANSFERENCIA DE INVENTARIO= INLOG_{parameters.Consecutive}";
                    var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                    //respuesta.Data.Mensaje = !siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription) ? siesaResponse.MessageDescription : siesaResponse.ResponseMessage;

                    if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                    {
                        respuesta.AddError(siesaResponse.MessageDescription);
                        respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                        return respuesta;
                    }
                    respuesta.Data.Exito = siesaResponse.Success;
                    respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                    _inputParameterRepository.Update(parameters);
                }
                else
                {
                    var resp = _processDataBatchRepository.Insert(_processDataStock);
                    if (!resp)
                    {
                        respuesta.AddError("Error to Insert in table ProcessDataStock");
                        respuesta.Data.Errors.Add("Error to Insert in table ProcessDataStock");
                        return respuesta;
                    }
                    respuesta.Data.Exito = true;
                    respuesta.Data.Mensaje = "OK";
                }
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoDevolucionesDeProveedor(SupplierReturnConfirmationDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }
                var f350_id_co = data.ConfDevolucionProveedor.Almace;
                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWsDevProveedor" && t.CO == f350_id_co);
                if (inputParameter == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                parameters.Consecutive = parameters.Consecutive.Value + 1;

                var f350_id_tipo_docto = parameters.DocumentType;
                var f350_consec_docto = parameters.Consecutive.Value.ToString();
                var f350_fecha = data.ConfDevolucionProveedor.Fecdev;
                var f451_id_tercero_comprador = data.ConfDevolucionProveedor.Numdoc;
                var docto = data.ConfDevolucionProveedor.Proext.Split('-');
                string f350_id_tercero;
                string f451_id_sucursal_prov;

                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                isLote = interfaceReceptorSr.Status;

                try
                {
                    f350_id_tercero = docto[0];
                    f451_id_sucursal_prov = docto[1];
                }
                catch (Exception ex)
                {
                    throw new Exception("Error a leer esta propiedad Pedext");
                }

                var docSection = new SectionData
                {
                    KeyName = "documentosV2",
                    Fields = new List<FieldData>
                    {
                        new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                        new FieldData { KeyName = "f350_id_co",                Value=f350_id_co},
                        new FieldData { KeyName = "f350_id_tipo_docto",        Value=f350_id_tipo_docto},
                        new FieldData { KeyName = "f350_consec_docto",         Value=f350_consec_docto},
                        new FieldData { KeyName = "f350_fecha",                Value=f350_fecha},
                        new FieldData { KeyName = "f350_id_tercero",           Value=f350_id_tercero},
                        new FieldData { KeyName = "f451_id_sucursal_prov",     Value=f451_id_sucursal_prov},
                        new FieldData { KeyName = "f451_id_tercero_comprador", Value=f451_id_tercero_comprador},
                    }
                };

                var settingStorage = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllDevolucionProveedor", data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var query = settingStorage.Value;
                int f421_nro_registro = 1;

                foreach (var item in data.ConfLineaDevolucionProveedor.LineasDevolucionProveedor)
                {
                    query = string.Format(query, item.Articu, data.SiesaCode, string.Empty);

                    var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                    .GetQuery<DevolucionProveedoresDto>(query,
                                                            compnayRepository.Server,
                                                            compnayRepository.DataBase,
                                                            compnayRepository.UserName,
                                                            compnayRepository.Password);
                    if (!plainBuilderManagerDetailSr.Status)
                    {
                        //throw new Exception("la consulta fallo");
                        continue;
                    }
                    var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.FirstOrDefault();
                    var f470_id_bodega = string.Empty;
                    var f470_id_un_movto = plainBuilderManagerDetail.id_un_movto;
                    var f017_dec_compra_venta = plainBuilderManagerDetail.dec_compra_venta;
                    decimal f470_vlr_bruto = Math.Round(Convert.ToDecimal(item.Canrea) * Convert.ToDecimal(plainBuilderManagerDetail.costo_uni), f017_dec_compra_venta);

                    if (item.Aptnap.ToUpper().Equals("AP"))
                    {
                        f470_id_bodega = plainBuilderManagerDetail.id_bodega_AP;
                    }
                    if (item.Aptnap.ToUpper().Equals("NA"))
                    {
                        f470_id_bodega = plainBuilderManagerDetail.id_bodega_NA;
                    }
                    if (string.IsNullOrEmpty(f470_id_bodega))
                    {
                        throw new Exception($"El valor sobre el campo Aptnap({item.Aptnap}) no corresponde a ningun valor esperado para calcular la bodega ");
                    }

                    var lote = string.Empty;
                    if (isLote)
                    {
                        if (!string.IsNullOrEmpty(item.LoteFa))
                        {
                            lote = item.LoteFa.Length > 15
                           ? item.LoteFa.Substring(item.LoteFa.Length - 15, 15)
                           : item.LoteFa;
                        }
                    }

                    movementsSection.Add(new SectionData
                    {
                        KeyName = "movimientosV13",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                            new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                            new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                            new FieldData { KeyName = "f470_id_co_movto",        Value=f350_id_co},
                            new FieldData { KeyName = "f470_id_bodega",          Value=f470_id_bodega},
                            new FieldData { KeyName = "f470_id_unidad_medida",   Value=plainBuilderManagerDetail.id_unidad_medida},
                            new FieldData { KeyName = "f470_cant_base",          Value=item.Canrea},
                            new FieldData { KeyName = "f470_id_item",            Value=item.Articu},
                            new FieldData { KeyName = "f470_id_lote",            Value=lote},
                            new FieldData { KeyName = "f470_id_un_movto",        Value=f470_id_un_movto},
                            new FieldData { KeyName = "f470_vlr_bruto",          Value=f470_vlr_bruto.ToString()}
                        }
                    });
                    f421_nro_registro++;
                }

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.AddRange(movementsSection);

                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO DEVOLUCION A PROVEEDOR= INLOG_{f421_nro_registro}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                //respuesta.Data.Mensaje = !siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription) ? siesaResponse.MessageDescription : siesaResponse.ResponseMessage;

                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                _inputParameterRepository.Update(parameters);
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoEntradaInventario(ConfirmationStockAdjustmentDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();
            List<ProcessDataStock> _processDataStock = new List<ProcessDataStock>();
            try
            {
                var configurationData = _configurationRepository.Get("QDM.Inlog.WebService.SendingStockBatch", data.CompanyId);
                if (configurationData == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var f470_id_bodega = string.Empty;
                if (data.ConfRegularizacionStock.Causal.ToUpper().Equals("EI") && data.ConfRegularizacionStock.Codaju.Equals("01"))
                    f470_id_bodega = "001";
                if (data.ConfRegularizacionStock.Causal.ToUpper().Equals("EN") && data.ConfRegularizacionStock.Codaju.Equals("EN"))
                    f470_id_bodega = "002";

                if (string.IsNullOrEmpty(f470_id_bodega))
                {
                    //throw new Exception($"No se pudo calcular la bodega con esta valor Causa:{data.ConfRegularizacionStock.Causal}, Codaju:{data.ConfRegularizacionStock.Codaju}");
                    return respuesta;
                }
                var f350_id_co = data.ConfRegularizacionStock.Almace;
                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }

                var parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWsEntradasInv" && t.CO == f350_id_co);
                if (parameters == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var settingStorage = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllCambioSituacionLogica", data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                parameters.Consecutive = parameters.Consecutive.Value + 1;
                var f350_id_tipo_docto = parameters.DocumentType;
                var f350_consec_docto = parameters.Consecutive.Value.ToString();
                var f350_fecha = DateTime.Now.ToString("yyyyMMdd");



                var docSection = new SectionData
                {
                    KeyName = "documentosV2",
                    Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f350_id_co",                Value=f350_id_co},
                            new FieldData { KeyName = "f350_id_tipo_docto",        Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "f350_consec_docto",         Value=f350_consec_docto},
                            new FieldData { KeyName = "f350_fecha",                Value=f350_fecha},
                        }
                };

                var query = settingStorage.Value;
                query = string.Format(query,
                         data.ConfRegularizacionStock.Articu,
                         data.SiesaCode,
                         string.Empty);

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<CambioSituacionLogicaDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }
                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.FirstOrDefault();
                int f421_nro_registro = 1;
                var f470_id_unidad_medida = plainBuilderManagerDetail.id_unidad_medida;
                movementsSection.Add(new SectionData
                {
                    KeyName = "movimientosV5",
                    Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f470_id_co",              Value=f350_id_co},
                            new FieldData { KeyName = "f470_id_tipo_docto",      Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "f470_consec_docto",       Value=f350_consec_docto},
                            new FieldData { KeyName = "f470_nro_registro",       Value=f421_nro_registro.ToString()},
                            new FieldData { KeyName = "f470_id_bodega",          Value=f470_id_bodega},
                            new FieldData { KeyName = "f470_id_co_movto",        Value=f350_id_co},
                            new FieldData { KeyName = "f470_id_unidad_medida",   Value=f470_id_unidad_medida},
                            new FieldData { KeyName = "f470_cant_base",          Value=data.ConfRegularizacionStock.Cantid},
                            new FieldData { KeyName = "f470_id_item",            Value=data.ConfRegularizacionStock.Articu},
                        }
                });

                //_processDataStock.Add(new ProcessDataStock
                //{
                //    f470_id_co = f350_id_co,
                //    f470_id_bodega = f470_id_bodega,
                //    f470_id_co_movto = f350_id_co,
                //    f470_id_unidad_medida = f470_id_unidad_medida,
                //    f470_cant_base = data.ConfRegularizacionStock.Cantid,
                //    f470_id_item = data.ConfRegularizacionStock.Articu,
                //    KeyName = "QDM.Inlog.Query.GetAllCambioSituacionLogica",
                //    PlainKeyName = "QDM.Siesa.PlainWsEntradasInv",
                //    SiesaCode = data.SiesaCode.ToString(),
                //    Status = StatusProcessDataStock.PENDING,
                //    CreatedDate = DateTime.Now,
                //    KeyHeader = "documentosV2",
                //    KeyItems = "movimientosV5",
                //    Source= "ConfirmationStockAdjustment"
                //});

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.AddRange(movementsSection);
                //configurationData.Value = "0";
                //if (bool.TryParse(configurationData.Value, out bool r))
                //{
                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO ENTRADA INVENTARIO= INLOG_{parameters.Consecutive}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                //respuesta.Data.Mensaje = !siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription) ? siesaResponse.MessageDescription : siesaResponse.ResponseMessage;

                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                _inputParameterRepository.Update(parameters);
                //}
                //else
                //{
                //    var resp = _processDataBatchRepository.Insert(_processDataStock);
                //    if (!resp)
                //    {
                //        respuesta.AddError("Error to Insert in table ProcessDataStock");
                //        respuesta.Data.Errors.Add("Error to Insert in table ProcessDataStock");
                //        return respuesta;
                //    }
                //    respuesta.Data.Exito = true;
                //    respuesta.Data.Mensaje = "OK";
                //}
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoCuadredeFacturas(RecibirConfCargaCamionDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();

            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }
                var f585_id_co = data.ConfCargaCamion.Centro;
                var parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWsPlanillaCuadreFactura" && t.CO == f585_id_co);
                if (parameters == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var settingStorage = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllCargaCamion", data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var query = settingStorage.Value;
                query = string.Format(query,
                    data.ConfCargaCamion.Matcot,
                    data.SiesaCode,
                    string.Empty);

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<CuadreFacturaDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }

                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.FirstOrDefault();
                parameters.Consecutive = parameters.Consecutive.Value + 1;

                var f585_id_tipo_docto = parameters.DocumentType;
                var f585_consec_docto = parameters.Consecutive.Value.ToString();
                var f585_fecha = data.ConfCargaCamion.Feccar;
                var f595_id_vehiculo = data.ConfCargaCamion.Matcot;

                var f585_id_tercero_transp = plainBuilderManagerDetail.id_tercero_transp;
                var f585_id_tercero_conductor = plainBuilderManagerDetail.id_tercero_conductor;
                var f585_nombre_conductor = plainBuilderManagerDetail.nombre_conductor;
                var f585_identif_conductor = plainBuilderManagerDetail.identif_conductor;


                var docSection = new SectionData
                {
                    KeyName = "PlanillaDocumentosV2",
                    Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f585_id_co",                Value=f585_id_co},
                            new FieldData { KeyName = "f585_id_tipo_docto",        Value=f585_id_tipo_docto},
                            new FieldData { KeyName = "f585_consec_docto",         Value=f585_consec_docto},
                            new FieldData { KeyName = "f585_fecha",                Value=f585_fecha},
                            new FieldData { KeyName = "f595_id_vehiculo",          Value=f595_id_vehiculo},
                            new FieldData { KeyName = "f585_id_tercero_transp",    Value=f585_id_tercero_transp},
                            new FieldData { KeyName = "f585_id_tercero_conductor", Value=f585_id_tercero_conductor},
                            new FieldData { KeyName = "f585_nombre_conductor",     Value=f585_nombre_conductor},
                            new FieldData { KeyName = "f585_identif_conductor",    Value=f585_identif_conductor},
                        }
                };

                //var query = settingStorage.Value;
                int f421_nro_registro = 1;
                foreach (var item in data.ConfLineaCargaCamion)
                {
                    //query = string.Format(query,
                    //    data.ConfCargaCamion.Volume,
                    //    data.SiesaCode,
                    //    string.Empty);

                    //var plainBuilderManagerDetail = _plainBuilderManagerRepository
                    //                                .GetQuery<CuadreFacturaDto>(query,
                    //                                        compnayRepository.Server,
                    //                                        compnayRepository.DataBase,
                    //                                        compnayRepository.UserName,
                    //                                        compnayRepository.Password).FirstOrDefault();
                    //if (plainBuilderManagerDetail == null)
                    //{
                    //    throw new Exception("la consulta fallo");
                    //}


                    //var f470_id_unidad_medida = plainBuilderManagerDetail.id_unidad_medida;
                    var f593_id_co_factura = string.Empty;
                    var f470_id_bodega = string.Empty;
                    var f350_id_co = string.Empty;
                    try
                    {
                        var pedext = item.Pedext.Split('-');
                        f593_id_co_factura = pedext[0];
                        f470_id_bodega = pedext[1];
                        f350_id_co = pedext[2];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error obtener la informacion de este campo {item.Pedext}");
                    }


                    movementsSection.Add(new SectionData
                    {
                        KeyName = "PlanillamovimientosV1",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                      Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f593_id_co",                 Value=f585_id_co},
                            new FieldData { KeyName = "f593_id_tipo_docto",         Value=f585_id_tipo_docto},
                            new FieldData { KeyName = "f593_consec_docto",          Value=f585_consec_docto},
                            new FieldData { KeyName = "f593_id_co_factura",         Value=f593_id_co_factura},
                            new FieldData { KeyName = "f593_id_tipo_docto_factura", Value=f470_id_bodega},
                            new FieldData { KeyName = "f593_consec_docto_factura",  Value=f350_id_co},
                        }
                    });
                    f421_nro_registro++;
                }

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.AddRange(movementsSection);

                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO PLANILLA DE CUADRE DE FACTURAS= INLOG_{parameters.Consecutive}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                //respuesta.Data.Mensaje = !siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription) ? siesaResponse.MessageDescription : siesaResponse.ResponseMessage;

                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                _inputParameterRepository.Update(parameters);
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoEntidades(CustomerReturnConfirmationDto data, string f350_id_tipo_docto,
                                                                                                            string f350_consec_docto,
                                                                                                            string f753_dato_fecha_hora,
                                                                                                            string f350_id_co,
                                                                                                            string f430_id_tipo_docto,
                                                                                                            string f430_consec_docto,
                                                                                                            InputParameter parameters,
                                                                                                            List<Configuration> configuration)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                //Dato EntidadesNotasCredito1
                var EntidadesNotasCredito1 = new SectionData
                {
                    KeyName = "EntidadesNotasCredito1",
                    Fields = new List<FieldData>
                    {
                        new FieldData { KeyName = "F_CIA",              Value=data.SiesaCode.ToString()},
                        new FieldData { KeyName = "f350_id_co",         Value=f350_id_co},
                        new FieldData { KeyName = "f350_id_tipo_docto", Value=f350_id_tipo_docto},
                        new FieldData { KeyName = "f350_consec_docto",  Value=f350_consec_docto}
                    }
                };


                //Dato EntidadesNotasCredito2
                var EntidadesNotasCredito2 = new SectionData
                {
                    KeyName = "EntidadesNotasCredito2",
                    Fields = new List<FieldData>
                    {
                     new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                     new FieldData { KeyName = "f350_id_co",              Value=f350_id_co},
                     new FieldData { KeyName = "f350_id_tipo_docto",      Value=f350_id_tipo_docto},
                     new FieldData { KeyName = "f350_consec_docto",       Value=f350_consec_docto},
                     new FieldData { KeyName = "f753_dato_texto",         Value=f350_id_co},
                    }
                };

                //Dato EntidadesNotasCredito3
                var EntidadesNotasCredito3 = new SectionData
                {
                    KeyName = "EntidadesNotasCredito3",
                    Fields = new List<FieldData>
                    {
                     new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                     new FieldData { KeyName = "f350_id_co",              Value=f350_id_co},
                     new FieldData { KeyName = "f350_id_tipo_docto",      Value=f350_id_tipo_docto},
                     new FieldData { KeyName = "f350_consec_docto",       Value=f350_consec_docto},
                     new FieldData { KeyName = "f753_dato_texto",         Value=f430_id_tipo_docto},
                    }
                };

                //Dato EntidadesNotasCredito4
                var EntidadesNotasCredito4 = new SectionData
                {
                    KeyName = "EntidadesNotasCredito4",
                    Fields = new List<FieldData>
                    {
                     new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                     new FieldData { KeyName = "f350_id_co",              Value=f350_id_co},
                     new FieldData { KeyName = "f350_id_tipo_docto",      Value=f350_id_tipo_docto},
                     new FieldData { KeyName = "f350_consec_docto",       Value=f350_consec_docto},
                     new FieldData { KeyName = "f753_dato_texto",         Value=f430_consec_docto},
                    }
                };

                //Dato EntidadesNotasCredito5
                var EntidadesNotasCredito5 = new SectionData
                {
                    KeyName = "EntidadesNotasCredito5",
                    Fields = new List<FieldData>
                    {
                     new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                     new FieldData { KeyName = "f350_id_co",              Value=f350_id_co},
                     new FieldData { KeyName = "f350_id_tipo_docto",      Value=f350_id_tipo_docto},
                     new FieldData { KeyName = "f350_consec_docto",       Value=f350_consec_docto}
                    }
                };

                //Dato EntidadesNotasCredito6
                var EntidadesNotasCredito6 = new SectionData
                {
                    KeyName = "EntidadesNotasCredito6",
                    Fields = new List<FieldData>
                    {
                     new FieldData { KeyName = "F_CIA",                   Value=data.SiesaCode.ToString()},
                     new FieldData { KeyName = "f350_id_co",              Value=f350_id_co},
                     new FieldData { KeyName = "f350_id_tipo_docto",      Value=f350_id_tipo_docto},
                     new FieldData { KeyName = "f350_consec_docto",       Value=f350_consec_docto},
                     new FieldData { KeyName = "f753_dato_fecha_hora",    Value=f753_dato_fecha_hora},
                    }
                };

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = "QDM.Siesa.PlainWsEntidadesNotasCredito",
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(EntidadesNotasCredito1);
                plainOC.Sections.Add(EntidadesNotasCredito2);
                plainOC.Sections.Add(EntidadesNotasCredito3);
                plainOC.Sections.Add(EntidadesNotasCredito4);
                plainOC.Sections.Add(EntidadesNotasCredito5);
                plainOC.Sections.Add(EntidadesNotasCredito6);

                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO ENTIDADES = INLOG_{parameters.Consecutive.Value}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }
            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoLotes(RecibirConfRecepcionDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var lotes = data.ConfLineaRecep.ConfLineaRecepcion.Where(m => !string.IsNullOrEmpty(m.LoteFa));
                if (!lotes.Any())
                {
                    throw new Exception("No existes item de lote para procesar...");
                }
                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var consecutivo = 1;
                var f403_fecha_creacion = DateTime.Now.ToString("yyyyMMdd");
                foreach (var item in lotes)
                {
                    string lotefa = item.LoteFa;

                    if (!string.IsNullOrEmpty(item.LoteFa))
                    {
                        if (item.LoteFa.Length > 15)
                            lotefa = item.LoteFa.Substring(item.LoteFa.Length - 15, 15);
                    }

                    movementsSection.Add(new SectionData
                    {
                        KeyName = "LotesV2",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                  Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f403_id",                Value=lotefa},
                            new FieldData { KeyName = "f403_id_item",           Value=item.Articu},
                            new FieldData { KeyName = "f403_fecha_creacion",   Value=f403_fecha_creacion},
                            new FieldData { KeyName = "f403_fecha_vcto",        Value=item.Feccad},
                            new FieldData { KeyName = "f403_fabricante",        Value=item.LoteFa},
                            new FieldData { KeyName = "f403_notas",             Value=item.LoteFa},
                        }
                    });
                    consecutivo++;
                }

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = "QDM.Siesa.PlainWsLotes",
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.AddRange(movementsSection);
                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }

                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO ENTRADA LOTES= INLOG_{consecutivo}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);

                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                    return respuesta;
                }
                respuesta.Data.Exito = siesaResponse.Success;
                respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoCompromisosPedidoVenta(PreparationConfirmationDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> docSection = new List<SectionData>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var F_CIA = data.SiesaCode;
                var docto = data.ConfPreparacion.Pedext.Split('-');

                string f350_id_co;
                string f420_id_tipo_docto;
                string f420_consec_docto;
                string f470_id_lote = string.Empty;

                try
                {
                    f350_id_co = docto[0];
                    f420_id_tipo_docto = docto[1];
                    f420_consec_docto = docto[2];
                }
                catch (Exception ex)
                {
                    throw new Exception("Error a leer esta propiedad Pedext");
                }


                var settingStorage = configuration.FirstOrDefault(t => t.Name == "QDM.Inlog.Query.GetAllCompromisoPedidoVenta" && t.Id_Cia == data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var query = settingStorage.Value;
                query = string.Format(query,
                         f350_id_co,
                         f420_id_tipo_docto,
                         f420_consec_docto,
                         data.SiesaCode,
                         string.Empty);

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerSr = _plainBuilderManagerRepository
                                                .GetQuery<PreparacionCabeceraDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerSr.Status)
                {
                    throw new Exception(plainBuilderManagerSr.Errors.FirstOrDefault().ErrorDetail);
                }
                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                if (!interfaceReceptorSr.Status)
                {
                    isLote = false;
                }
                isLote = interfaceReceptorSr.Data.IsStatus;


                var result = data.ConfLineaPrep.LineasPreparacion
                             .GroupBy(x => isLote
                                 ? new { x.Pedext, x.Lote, x.Articu, x.Codlin }
                                 : new { x.Pedext, Lote = (string)null, x.Articu, x.Codlin }) // Usa null para Lote si isLote es falso
                             .Select(g => new
                             {
                                 Pedext = g.Key.Pedext,
                                 Lote = g.Key.Lote, // Cuando isLote es falso, este valor será null
                                 Articu = g.Key.Articu,
                                 Codlin = g.Key.Codlin,
                                 Items = g.ToList(),
                                 Cant = g.Sum(x => int.Parse(x.Canrec)).ToString()
                             })
                             .ToList();

                var consecutivo = 0;
                foreach (var item in result)
                {
                    var plainBuilderManager = plainBuilderManagerSr.Data.Where(t => t.rowidright == item.Codlin && t.id_item == item.Articu).FirstOrDefault();
                    if (plainBuilderManager == null)
                        continue;

                    var f431_id_bodega = plainBuilderManager.id_bodega;
                    var f431_id_unidad_medida = plainBuilderManager.id_unidad_medida;
                    var f431_nro_registro = plainBuilderManager.rowid;
                    var lote = string.Empty;
                    if (isLote)
                    {
                        lote = item.Lote.Length > 15
                       ? item.Lote.Substring(item.Lote.Length - 15, 15)
                       : item.Lote;
                    }

                    docSection.Add(new SectionData
                    {
                        KeyName = "PedidosCompromisosV6",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                        Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f430_id_co",                   Value=f350_id_co},
                            new FieldData { KeyName = "f430_id_tipo_docto",           Value=f420_id_tipo_docto},
                            new FieldData { KeyName = "f430_consec_docto",            Value=f420_consec_docto},
                            new FieldData { KeyName = "f431_id_item",                 Value=item.Articu},
                            new FieldData { KeyName = "f431_id_bodega",               Value=f431_id_bodega},
                            new FieldData { KeyName = "f431_id_unidad_medida",        Value=f431_id_unidad_medida},
                            new FieldData { KeyName = "f431_nro_registro",            Value=f431_nro_registro},
                            new FieldData { KeyName = "f431_id_lote",                 Value=lote},
                            new FieldData { KeyName = "f431_cant_base",               Value=item.Cant},
                            new FieldData { KeyName = "f405_cant_por_remisionar_base",Value=item.Cant},
                        }
                    });
                    consecutivo++;
                }
                if (docSection.Count > 0)
                {
                    var plainOC = new PlainDataInput
                    {
                        Company = data.SiesaCode,
                        KeyName = "QDM.Siesa.PlainWsPedidosCompromisos",
                        Sections = new List<SectionData>()
                    };
                    plainOC.Sections.AddRange(docSection);

                    var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                    try
                    {
                        var fileGenerate = newPlain.File;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                    }

                    string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                    string responseMessage = $"PLANO COMPROMISO PEDIDO VENTA= INLOG_{consecutivo}";
                    var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                    if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                    {
                        respuesta.AddError(siesaResponse.MessageDescription);
                        respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                        return respuesta;
                    }
                    respuesta.Data.Exito = siesaResponse.Success;
                    respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                }
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoRemisionarPedidosDeVenta(PreparationConfirmationDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            List<SectionData> movementsSection = new List<SectionData>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }

                var docto = data.ConfPreparacion.Pedext.Split('-');
                var F_CIA = data.SiesaCode;
                string F350_ID_CO;
                string F430_ID_TIPO_DOCTO;
                string F430_CONSEC_DOCTO;

                var F350_FECHA = DateTime.Now.ToString("yyyyMMdd");
                try
                {
                    F350_ID_CO = docto[0];
                    F430_ID_TIPO_DOCTO = docto[1];
                    F430_CONSEC_DOCTO = docto[2];
                }
                catch (Exception ex)
                {
                    throw new Exception("Error a leer esta propiedad Pedext");
                }

                var parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWs.GetRemisionPedidoVenta" && t.CO == F350_ID_CO);
                if (parameters == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                parameters.Consecutive = parameters.Consecutive.Value + 1;
                string F350_ID_TIPO_DOCTO = parameters.DocumentType;
                string F350_CONSEC_DOCTO = parameters.Consecutive.Value.ToString();

                var docSection = new SectionData
                {
                    KeyName = "RemisionesComercialDoctoV3",
                    Fields = new List<FieldData>
                    {
                        new FieldData { KeyName = "F_CIA",              Value=data.SiesaCode.ToString()},
                        new FieldData { KeyName = "F350_ID_CO",         Value=F350_ID_CO},
                        new FieldData { KeyName = "F350_ID_TIPO_DOCTO", Value=F350_ID_TIPO_DOCTO},
                        new FieldData { KeyName = "F350_CONSEC_DOCTO",  Value=F350_CONSEC_DOCTO},
                        new FieldData { KeyName = "F350_FECHA",         Value=F350_FECHA},
                        new FieldData { KeyName = "F430_ID_TIPO_DOCTO", Value=F430_ID_TIPO_DOCTO},
                        new FieldData { KeyName = "F430_CONSEC_DOCTO",  Value=F430_CONSEC_DOCTO},
                    }
                };


                var settingStorage = configuration.FirstOrDefault(t => t.Name == "QDM.Inlog.Query.GetAllCompromisoPedidoVenta" && t.Id_Cia == data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var query = settingStorage.Value;
                query = string.Format(query,
                         F350_ID_CO,
                         F430_ID_TIPO_DOCTO,
                         F430_CONSEC_DOCTO,
                         data.SiesaCode,
                         string.Empty);

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerSr = _plainBuilderManagerRepository
                                                .GetQuery<PreparacionCabeceraDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerSr.Status)
                {
                    throw new Exception(plainBuilderManagerSr.Errors.FirstOrDefault().ErrorDetail);
                }
                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                if (!interfaceReceptorSr.Status)
                {
                    isLote = false;
                }
                isLote = interfaceReceptorSr.Data.IsStatus;


                var result = data.ConfLineaPrep.LineasPreparacion
                             .GroupBy(x => isLote
                                 ? new { x.Pedext, x.Lote, x.Articu, x.Codlin }
                                 : new { x.Pedext, Lote = (string)null, x.Articu, x.Codlin }) // Usa null para Lote si isLote es falso
                             .Select(g => new
                             {
                                 Pedext = g.Key.Pedext,
                                 Lote = g.Key.Lote, // Cuando isLote es falso, este valor será null
                                 Articu = g.Key.Articu,
                                 Codlin = g.Key.Codlin,
                                 Items = g.ToList(),
                                 Cant = g.Sum(x => int.Parse(x.Canrec)).ToString()
                             })
                             .ToList();

                var consecutivo = 0;
                int f470_nro_registro = data.ConfLineaPrep.LineasPreparacion.Count;
                foreach (var item in result)
                {
                    var plainBuilderManager = plainBuilderManagerSr.Data.Where(t => t.rowidright == item.Codlin && t.id_item == item.Articu).FirstOrDefault();
                    if (plainBuilderManager == null)
                        continue;

                    var f470_id_bodega = plainBuilderManager.id_bodega;
                    var f470_id_unidad_medida = plainBuilderManager.id_unidad_medida;
                    var f470_id_motivo = plainBuilderManager.id_motivo;
                    var f470_id_ccosto_movto = plainBuilderManager.id_ccosto_movto;
                    var f470_id_lista_precio = plainBuilderManager.id_lista_precio;
                    var f470_id_un_movto = plainBuilderManager.id_un_movto;
                    var f470_id_co_movto = plainBuilderManager.id_co_movto;
                    var lote = string.Empty;
                    if (isLote)
                    {
                        lote = item.Lote.Length > 15
                       ? item.Lote.Substring(item.Lote.Length - 15, 15)
                       : item.Lote;
                    }
                    f470_nro_registro++;
                    movementsSection.Add(new SectionData
                    {
                        KeyName = "RemisionesComercialMovtoV11",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                 Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f470_id_co",            Value=F350_ID_CO},
                            new FieldData { KeyName = "f470_id_tipo_docto",    Value=F350_ID_TIPO_DOCTO},
                            new FieldData { KeyName = "f470_consec_docto",     Value=F350_CONSEC_DOCTO},
                            new FieldData { KeyName = "f470_nro_registro",     Value=f470_nro_registro.ToString()},
                            new FieldData { KeyName = "f470_id_lote",          Value=lote},
                            new FieldData { KeyName = "f470_id_bodega",        Value=f470_id_bodega},
                            new FieldData { KeyName = "f470_id_motivo",        Value=f470_id_motivo},
                            new FieldData { KeyName = "f470_id_ccosto_movto",  Value=f470_id_ccosto_movto},
                            new FieldData { KeyName = "f470_id_lista_precio",  Value=f470_id_lista_precio},
                            new FieldData { KeyName = "f470_id_un_movto",      Value=f470_id_un_movto},
                            new FieldData { KeyName = "f470_id_co_movto",      Value=f470_id_co_movto},
                            new FieldData { KeyName = "f470_id_unidad_medida", Value=f470_id_unidad_medida},
                            new FieldData { KeyName = "f470_cant_base",        Value=item.Cant},
                            new FieldData { KeyName = "f470_id_item",          Value=item.Articu},
                        }
                    });
                    consecutivo++;
                }
                if (movementsSection.Count > 0)
                {
                    var plainOC = new PlainDataInput
                    {
                        Company = data.SiesaCode,
                        KeyName = parameters.PlainKeyName,
                        Sections = new List<SectionData>()
                    };
                    plainOC.Sections.Add(docSection);
                    plainOC.Sections.AddRange(movementsSection);

                    var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                    try
                    {
                        var fileGenerate = newPlain.File;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                    }

                    string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                    string responseMessage = $"PLANO REMISION PEDIDO VENTA= INLOG_{consecutivo}";
                    var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                    if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                    {
                        respuesta.AddError(siesaResponse.MessageDescription);
                        respuesta.Data.Errors.Add(siesaResponse.MessageDescription);
                        return respuesta;
                    }
                    respuesta.Data.Exito = siesaResponse.Success;
                    respuesta.Data.Mensaje = siesaResponse.ResponseMessage;
                    _inputParameterRepository.Update(parameters);
                }
            }
            catch (Exception e)
            {
                respuesta.AddError(e);
            }

            return respuesta;
        }

        public ServiceResponse<EscribirPlanoOutput> PlanoAgruparArtículosOrdenFabricacion(OrdenFabricacionDto data)
        {
            var respuesta = new ServiceResponse<EscribirPlanoOutput>();
            respuesta.Data = new EscribirPlanoOutput();
            respuesta.Data.Exito = false;
            respuesta.Data.Errors = new List<string>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }
                var item = new ProcessConfOrdenFabricacion
                {
                    Articu = data.ConfOrdenFabricacion.Articu,
                    ArtPro = data.ConfOrdenFabricacion.Artpro,
                    Cantid = float.Parse(data.ConfOrdenFabricacion.Cantid),
                    CompanyId = data.CompanyId,
                    FecCad = data.ConfOrdenFabricacion.Feccad,
                    FecReg = data.ConfOrdenFabricacion.Fecreg,
                    GofExt = data.ConfOrdenFabricacion.Gofext,
                    LoteFa = data.ConfOrdenFabricacion.Lotefa,
                    OfrExt = data.ConfOrdenFabricacion.Ofrext,
                    SiesaCode = data.SiesaCode,
                    Source = "ProcessConfOrdenFabricacion",
                    Status = StatusProcess.PENDING
                };
                var resp = _processConfOrdenFabricacionRepository.Insert(item);
                if (!resp)
                {
                    respuesta.AddError("Error to Insert in table ProcessConfOrdenFabricacion");
                    respuesta.Data.Errors.Add("Error to Insert in table ProcessConfOrdenFabricacion");
                    return respuesta;
                }

                var respValid = ValidationPlanoAgruparArtículosOrdenFabricacion(item);
                if (respValid.Status)
                {
                    var respTrans = PlanoTransferenciaOrdenFabricacion(item);
                    if (!respTrans.Status)
                    {
                        respuesta.Data.Errors.Add(respTrans.Errors.FirstOrDefault().ErrorDetail);
                        return respuesta;
                    }

                    respTrans = PlanoCompromisoOrdenFabricacion(item);
                    if (!respTrans.Status)
                    {
                        respuesta.Data.Errors.Add(respTrans.Errors.FirstOrDefault().ErrorDetail);
                        return respuesta;
                    }
                }
                respuesta.Data.Exito = true;
                respuesta.Data.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                respuesta.AddError(ex);
            }

            return respuesta;
        }

        private ServiceResponse ValidationPlanoAgruparArtículosOrdenFabricacion(ProcessConfOrdenFabricacion data)
        {
            var respuesta = new ServiceResponse();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var configuration = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllOrdenFabricacion", data.CompanyId);
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                var query = configuration.Value;
                query = string.Format(query,
                         data.OfrExt,
                         data.SiesaCode,
                         string.Empty);

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaOrdenFabricacionDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }

                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.ToList();

                var listOrderFabricacion = _processConfOrdenFabricacionRepository.GetAll(t => t.Status == StatusProcess.PENDING
                                                                                            && t.OfrExt == data.OfrExt
                                                                                            && t.CompanyId == data.CompanyId
                                                                                            && t.SiesaCode == data.SiesaCode);
                var items = listOrderFabricacion
                            .GroupBy(p => p.Articu)
                            .Select(g => new EntradaOrdenFabricacionDto
                            {
                                id_item = g.Key,
                                cant_base = g.Sum(p => p.Cantid)
                            })
                            .ToList();
                if (plainBuilderManagerDetail == null || plainBuilderManagerDetail.Count == 0)
                {
                    throw new Exception("No tengo registros en Siesa");
                }

                if (items.Count == plainBuilderManagerDetail.Count)
                {
                    var lista1Agrupada = plainBuilderManagerDetail.GroupBy(x => x.id_item)
                                .ToDictionary(g => g.Key, g => g.Sum(x => x.cant_base));

                    var lista2Agrupada = items.GroupBy(x => x.id_item)
                                               .ToDictionary(g => g.Key, g => g.Sum(x => x.cant_base));
                    respuesta.Data = lista1Agrupada.Count == lista2Agrupada.Count && !lista1Agrupada.Except(lista2Agrupada).Any();
                }


            }
            catch (Exception ex)
            {
                respuesta.AddError(ex);
            }
            return respuesta;
        }

        private ServiceResponse PlanoTransferenciaOrdenFabricacion(ProcessConfOrdenFabricacion data)
        {
            var respuesta = new ServiceResponse();
            List<SectionData> movementsSection = new List<SectionData>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var inputParameter = _inputParameterRepository.GetByCompanyAll(data.CompanyId);
                if (inputParameter == null || inputParameter.Count == 0)
                {
                    throw new Exception($"los parametros generales no se encuentran configurados {data.CompanyId}");
                }
                var f350_id_co = string.Empty;
                var parameters = inputParameter.FirstOrDefault(t => t.PlainKeyName == "QDM.Siesa.PlainWsTransferenciasInv" && t.CO == f350_id_co);
                if (parameters == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                var settingStorage = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllOrdenFabricacion", data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                var query = settingStorage.Value;
                query = string.Format(query,
                         data.OfrExt,
                         data.SiesaCode,
                         string.Empty);

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaOrdenFabricacionDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerSr.Status)
                {
                    throw new Exception(plainBuilderManagerSr.Errors.FirstOrDefault().ErrorDetail);
                }
                var plainBuilderManager = plainBuilderManagerSr.Data.FirstOrDefault();
                parameters.Consecutive = parameters.Consecutive.Value + 1;
                var f350_id_tipo_docto = parameters.DocumentType;
                var f350_consec_docto = parameters.Consecutive.Value.ToString();
                var f450_id_bodega_salida = plainBuilderManager.id_bodega_salida;
                var f450_id_bodega_entrada = plainBuilderManager.id_bodega_entrada;
                var f350_fecha = DateTime.Now.ToString("yyyyMMdd");

                var docSection = new SectionData
                {
                    KeyName = "documentosV2",
                    Fields = new List<FieldData>
                    {
                        new FieldData { KeyName = "F_CIA",                     Value=data.SiesaCode.ToString()},
                        new FieldData { KeyName = "f350_id_co",                Value=f350_id_co},
                        new FieldData { KeyName = "f350_id_tipo_docto",        Value=f350_id_tipo_docto},
                        new FieldData { KeyName = "f350_consec_docto",         Value=f350_consec_docto},
                        new FieldData { KeyName = "f350_fecha",                Value=f350_fecha},
                        new FieldData { KeyName = "f450_id_bodega_salida",     Value=f450_id_bodega_salida},
                        new FieldData { KeyName = "f450_id_bodega_entrada",    Value=f450_id_bodega_entrada},
                    }
                };

                int f421_nro_registro = 1;
                settingStorage = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllOrdenFabricacion", data.CompanyId);
                if (inputParameter == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                query = settingStorage.Value;
                query = string.Format(query,
                         data.OfrExt,
                         data.SiesaCode,
                         string.Empty);

                compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaOrdenFabricacionDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }

                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.ToList();
                var checkDetail = new ServiceResponse();

                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                if (!interfaceReceptorSr.Status)
                {
                    isLote = false;
                }
                isLote = interfaceReceptorSr.Data.IsStatus;
                var items = _processConfOrdenFabricacionRepository.GetAll(t => t.Status == StatusProcess.PENDING
                                                                                            && t.OfrExt == data.OfrExt
                                                                                            && t.CompanyId == data.CompanyId
                                                                                            && t.SiesaCode == data.SiesaCode);

                var lines = items
                           .GroupBy(x => isLote
                               ? new { x.OfrExt, x.LoteFa, x.Articu }
                               : new { x.OfrExt, LoteFa = (string)null, x.Articu })
                           .Select(g => new
                           {
                               OfrExt = g.Key.OfrExt,
                               Articu = g.Key.Articu,
                               LoteFa = g.Key.LoteFa,
                               Items = g.ToList(),
                               TotalCantco = g.Sum(t => t.Cantid),
                           })
                           .ToList();

                foreach (var item in lines)
                {
                    var artTemp = plainBuilderManagerDetail.Where(t => t.id_item == item.Articu);

                    if (!artTemp.Any())
                    {
                        checkDetail.AddError(new Exception($"Para este articulo {item.Articu} no se  encontre ninguna realacion, por favor validar..."));
                        continue;
                    }
                    var art = artTemp.FirstOrDefault();
                    //TODO:todas la consulta a base de datos
                    var f470_id_unidad_medida = art.id_unidad_medida;
                    var f470_id_un_movto = art.id_un_movto;
                    var lote = string.Empty;
                    if (isLote)
                    {
                        if (!string.IsNullOrEmpty(item.LoteFa))
                        {
                            lote = item.LoteFa.Length > 15
                           ? item.LoteFa.Substring(item.LoteFa.Length - 15, 15)
                           : item.LoteFa;
                        }
                    }

                    f421_nro_registro++;
                    movementsSection.Add(new SectionData
                    {
                        KeyName = "movimientosV5",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f470_id_co",           Value=f350_id_co},
                            new FieldData { KeyName = "f470_id_tipo_docto",   Value=f350_id_tipo_docto},
                            new FieldData { KeyName = "f470_consec_docto",    Value=f350_consec_docto},
                            new FieldData { KeyName = "f470_nro_registro",    Value=f421_nro_registro.ToString()},
                            new FieldData { KeyName = "f470_id_bodega",       Value=f450_id_bodega_salida},
                            new FieldData { KeyName = "f470_id_lote",         Value=lote},
                            new FieldData { KeyName = "f470_id_co_movto",     Value=f350_id_co},
                            new FieldData { KeyName = "f470_id_unidad_medida",Value=f470_id_unidad_medida},
                            new FieldData { KeyName = "f470_id_un_movto",     Value=f470_id_un_movto},
                            new FieldData { KeyName = "f470_cant_base",       Value=item.TotalCantco.ToString()},
                            new FieldData { KeyName = "f470_id_item",         Value=item.Articu},
                        }
                    });
                }
                if (!checkDetail.Status)
                {
                    string msg = string.Join(", ", checkDetail.Errors.Select(p => $"{p.ErrorMessage}"));
                    throw new Exception(msg);
                }

                var plainOC = new PlainDataInput
                {
                    Company = data.SiesaCode,
                    KeyName = parameters.PlainKeyName,
                    Sections = new List<SectionData>()
                };
                plainOC.Sections.Add(docSection);
                plainOC.Sections.AddRange(movementsSection);

                var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                try
                {
                    var fileGenerate = newPlain.File;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                }
                var configuration = _settingStorageRepository.GetAll();
                if (configuration == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }
                string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                string responseMessage = $"PLANO TRANSFERENCIA INVENTARIO ORDEN FABRICACION= INLOG_{f421_nro_registro}";
                var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);
                //respuesta.Data.Mensaje = !siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription) ? siesaResponse.MessageDescription : siesaResponse.ResponseMessage;

                if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                {
                    respuesta.AddError(siesaResponse.MessageDescription);
                    return respuesta;
                }
                _inputParameterRepository.Update(parameters);
            }
            catch (Exception ex)
            {
                respuesta.AddError(ex);
            }
            return respuesta;
        }

        public ServiceResponse PlanoCompromisoOrdenFabricacion(ProcessConfOrdenFabricacion data)
        {
            var respuesta = new ServiceResponse();
            List<SectionData> movementsSection = new List<SectionData>();
            try
            {
                if (data == null)
                {
                    throw new Exception("Debe ingresar los valores para los items");
                }

                var settingStorage = _settingStorageRepository.GetKeyAndCompany("QDM.Inlog.Query.GetAllOrdenFabricacion", data.CompanyId);
                if (settingStorage == null)
                {
                    throw new Exception("los parametros generales no se encuentran configurados");
                }

                var query = settingStorage.Value;
                query = string.Format(query,
                         data.OfrExt,
                         data.SiesaCode,
                         string.Empty);

                var compnayRepository = _companyRepository.Get(data.CompanyId);
                if (compnayRepository == null)
                {
                    throw new Exception("la compañia no se encuentran configurados");
                }

                var plainBuilderManagerDetailSr = _plainBuilderManagerRepository
                                                .GetQuery<EntradaOrdenFabricacionDto>(query,
                                                        compnayRepository.Server,
                                                        compnayRepository.DataBase,
                                                        compnayRepository.UserName,
                                                        compnayRepository.Password);
                if (!plainBuilderManagerDetailSr.Status)
                {
                    throw new Exception(plainBuilderManagerDetailSr.Errors.FirstOrDefault().ErrorDetail);
                }

                var plainBuilderManagerDetail = plainBuilderManagerDetailSr.Data.ToList();
                var consecutivo = 1;
                bool isLote = false;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
                if (!interfaceReceptorSr.Status)
                {
                    isLote = false;
                }
                isLote = interfaceReceptorSr.Data.IsStatus;
                var items = _processConfOrdenFabricacionRepository.GetAll(t => t.Status == StatusProcess.PENDING
                                                                                            && t.OfrExt == data.OfrExt
                                                                                            && t.CompanyId == data.CompanyId
                                                                                            && t.SiesaCode == data.SiesaCode);
                var lines = items
                           .GroupBy(x => isLote
                               ? new { x.OfrExt, x.LoteFa, x.Articu }
                               : new { x.OfrExt, LoteFa = (string)null, x.Articu })
                           .Select(g => new
                           {
                               OfrExt = g.Key.OfrExt,
                               Articu = g.Key.Articu,
                               LoteFa = g.Key.LoteFa,
                               Items = g.ToList(),
                               TotalCantco = g.Sum(t => t.Cantid),
                           })
                           .ToList();

                foreach (var item in lines)
                {
                    var db = plainBuilderManagerDetail.Where(t => t.id_item == item.Articu).FirstOrDefault();
                    var f850_id_co = string.Empty;
                    var f850_id_tipo_docto = string.Empty;
                    var f850_consec_docto = string.Empty;
                    var dcto = item.OfrExt.Split('-');
                    try
                    {
                        f850_id_co = dcto[0];
                        f850_id_tipo_docto = dcto[1];
                        f850_consec_docto = dcto[2];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error a leer esta propiedad OfrExt");
                    }
                    var lote = string.Empty;
                    if (isLote)
                    {
                        if (!string.IsNullOrEmpty(item.LoteFa))
                        {
                            lote = item.LoteFa.Length > 15
                           ? item.LoteFa.Substring(item.LoteFa.Length - 15, 15)
                           : item.LoteFa;
                        }
                    }
                    var section = new SectionData
                    {
                        KeyName = "OPCompromisosV12",
                        Fields = new List<FieldData>
                        {
                            new FieldData { KeyName = "F_CIA",                 Value=data.SiesaCode.ToString()},
                            new FieldData { KeyName = "f850_id_co",            Value=f850_id_co},
                            new FieldData { KeyName = "f850_id_tipo_docto",    Value=f850_id_tipo_docto},
                            new FieldData { KeyName = "f850_consec_docto",     Value=f850_consec_docto},
                            new FieldData { KeyName = "f850_nro_registro",     Value=db.rowid},
                            new FieldData { KeyName = "f860_id_bodega",        Value=db.id_bodega_entrada},
                            new FieldData { KeyName = "f860_id_unidad_medida", Value=db.id_unidad_medida},
                            new FieldData { KeyName = "f851_id_item_padre",    Value=db.id_item_padre},
                            new FieldData { KeyName = "f860_id_item_comp",     Value=data.Articu},
                            new FieldData { KeyName = "f860_id_lote",          Value=lote},
                            new FieldData { KeyName = "f860_cant_base",        Value=item.TotalCantco.ToString()},
                        }
                    };
                    movementsSection.Add(section);
                }
                if (movementsSection.Count > 0)
                {
                    var plainOC = new PlainDataInput
                    {
                        Company = data.SiesaCode,
                        KeyName = "QDM.Siesa.PlainWsLotes",
                        Sections = new List<SectionData>()
                    };
                    plainOC.Sections.AddRange(movementsSection);
                    var newPlain = _plainBuilderManagerServices.Execute(plainOC);
                    try
                    {
                        var fileGenerate = newPlain.File;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error sobre la generacion del plano {ex.ToString()}");
                    }
                    var configuration = _settingStorageRepository.GetAll();
                    if (configuration == null)
                    {
                        throw new Exception("los parametros generales no se encuentran configurados");
                    }
                    string xmlFileName = $"{plainOC.KeyName}-{DateTime.Now:yyyy-MM-ddHHmmss}.xml";
                    string responseMessage = $"PLANO COMPROMISO ORDEN FABRICACION= INLOG_{consecutivo}";
                    var siesaResponse = _siesaPlainConnector.SendPlan(newPlain.File, true, xmlFileName, data.CompanyId, data.SiesaCode, configuration, responseMessage);

                    if (!siesaResponse.Success && !string.IsNullOrEmpty(siesaResponse.MessageDescription))
                    {
                        respuesta.AddError(siesaResponse.MessageDescription);
                        return respuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.AddError(ex);
            }

            return respuesta;
        }
    }
}
