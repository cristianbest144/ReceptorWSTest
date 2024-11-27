using BusinessLogicLayer.Models;
using DataAccessLayer.Common;
using DataAccessLayer.Repository;
using Newtonsoft.Json;
using Services.Helper;
using Shared;
using System;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class ReceptionServices : IReceptionServices
    {
        public readonly EventLogRepository _eventLogRepository;
        public readonly SiesaServices _siesaServices;
        private readonly InterfaceReceptorRepository _interfaceReceptorRepository;
        public ReceptionServices()
        {
            _eventLogRepository = new EventLogRepository();
            _siesaServices = new SiesaServices();
            _interfaceReceptorRepository = new InterfaceReceptorRepository();

        }

        public ChangeLogicalSituationResponseDto ProcessChangeLogicalSituation(ChangeLogicalSituationDto data)
        {
            var resp = new ChangeLogicalSituationResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessChangeLogicalSituation", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }

                var respProcess = new EventlogHelper().ProcessEventLog<ChangeLogicalSituationDto>(data, "ProcessChangeLogicalSituation", trackingId, OperationType.Insert, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    return resp;
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<ChangeLogicalSituationDto>(data, $"PlanoTransferenciaInventario {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);

                var siesaSr = _siesaServices.PlanoTransferenciaInventario(data);
                if (!siesaSr.Status)
                {
                    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                    new EventlogHelper().ProcessEventLog<ChangeLogicalSituationDto>(data, $"Error: PlanoTransferenciaInventario {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }

                respProcess = new EventlogHelper().ProcessEventLog<ChangeLogicalSituationDto>(data, "ProcessChangeLogicalSituation", trackingId, OperationType.Update, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    new EventlogHelper().ProcessEventLog<ChangeLogicalSituationDto>(data, $"Error: ProcessChangeLogicalSituation {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }
                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<ChangeLogicalSituationDto>(data, $"Error: ProcessChangeLogicalSituation {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }

            return resp;
        }

        public ConfirmationStockAdjustmentResponseDto ProcessConfirmationStockAdjustment(ConfirmationStockAdjustmentDto data)
        {
            var resp = new ConfirmationStockAdjustmentResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessConfirmationStockAdjustment", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }

                var respProcess = new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, "ProcessConfirmationStockAdjustment", trackingId, OperationType.Insert, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    return resp;
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, $"ProcessConfirmationStockAdjustment {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);
                //new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, "ProcessConfirmationStockAdjustment esta interafce esta apagada", trackingId);
                //if (respProcess.Code == 11)
                //{
                //    resp.Mensaje = respProcess.Message;
                //    return resp;
                //}
                //resp.Codigo = 1;
                //resp.Mensaje = "Interface Apagada Temporalmente.";
                //return resp;

                bool isPlanoEntradaInventario = true;
                bool isPlanoTransferenciaInventario = true;
                bool isPlanoSalidaInventario = true;
                //var siesaSr = _siesaServices.PlanoEntradaInventario(data);
                //if (!siesaSr.Status)
                //{
                //    isPlanoEntradaInventario = false;
                //    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                //    new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, $"Error: PlanoEntradaInventario {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR);
                //    return resp;
                //}

                //if (siesaSr.Status && !siesaSr.Data.Errors.Any() && !siesaSr.Data.Exito)
                //{
                isPlanoEntradaInventario = false;
                var siesaSr = _siesaServices.PlanoTransferenciaInventario(new ChangeLogicalSituationDto
                {
                    CompanyId = data.CompanyId,
                    SiesaCode = data.SiesaCode,
                    Message = new MessageDto
                    {
                        MessageInfo = new MessageInfoDto
                        {
                            DateTime = data.Message.MessageInfo.DateTime,
                            OriginatorName = data.Message.MessageInfo.OriginatorName,
                        },
                        TrxId = data.Message.TrxId,
                    },
                    ConfCambioSituacionLogica = new ConfCambioSituacionLogicaDto
                    {
                        Accion = data.ConfRegularizacionStock.Accion,
                        Almace = data.ConfRegularizacionStock.Almace,
                        Articu = data.ConfRegularizacionStock.Articu,
                        Artpro = data.ConfRegularizacionStock.Artpro,
                        Artpv1 = data.ConfRegularizacionStock.Artpv1,
                        Artpv2 = data.ConfRegularizacionStock.Artpv2,
                        Artpvl = data.ConfRegularizacionStock.Artpvl,
                        Cantid = data.ConfRegularizacionStock.Cantid,
                        //Coment = data.ConfRegularizacionStock.Coment,
                        Feccad = data.ConfRegularizacionStock.Feccad,
                        //Feccam = data.ConfRegularizacionStock.Feccam,
                        Lotefa = data.ConfRegularizacionStock.Lotefa,
                        Propie = data.ConfRegularizacionStock.Propie,
                        Stnuev = data.ConfRegularizacionStock.Codaju,
                        Stviej = data.ConfRegularizacionStock.Causal,
                        Varia1 = data.ConfRegularizacionStock.Varia1,
                        Varia2 = data.ConfRegularizacionStock.Varia2,
                        Varlog = data.ConfRegularizacionStock.Varlog,
                        //Causal = data.ConfRegularizacionStock.Causal,
                        //Codaju = data.ConfRegularizacionStock.Codaju,
                    }
                }, Regulation.Regulatization);
                if (!siesaSr.Status)
                {
                    isPlanoTransferenciaInventario = false;
                    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                    new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, $"Error: PlanoTransferenciaInventario {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }
                //}

                //if (siesaSr.Status && !siesaSr.Data.Errors.Any() && !siesaSr.Data.Exito)
                //{
                //    isPlanoTransferenciaInventario = false;
                //    siesaSr = _siesaServices.PlanoSalidaInventario(data);
                //    if (!siesaSr.Status)
                //    {
                //        isPlanoSalidaInventario = false;
                //        resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                //        ProcessEventLog<ConfirmationStockAdjustmentDto>(data, $"Error: PlanoEntradaInventario {resp.Mensaje}", trackingId, OperationType.Update);
                //        return resp;
                //    }
                //}

                //if (siesaSr.Status && !siesaSr.Data.Errors.Any() && !siesaSr.Data.Exito)
                //{
                //    isPlanoSalidaInventario = false;
                //}

                //if (!isPlanoEntradaInventario && !isPlanoTransferenciaInventario && !isPlanoSalidaInventario)
                if (!isPlanoEntradaInventario && !isPlanoTransferenciaInventario)
                    throw new Exception($"No se realizo ningun proceso, la data entrante no cumplio {JsonConvert.SerializeObject(data)}");

                respProcess = new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, "ProcessConfirmationStockAdjustment", trackingId, OperationType.Update, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, $"Error: ProcessConfirmationStockAdjustment {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }

                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<ConfirmationStockAdjustmentDto>(data, $"Error: ProcessConfirmationStockAdjustment {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }

            return resp;
        }

        public CustomerReturnConfirmationResponseDto ProcessCustomerReturnConfirmation(CustomerReturnConfirmationDto data)
        {
            var resp = new CustomerReturnConfirmationResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessCustomerReturnConfirmation", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }

                var respProcess = new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, "ProcessCustomerReturnConfirmation", trackingId, OperationType.Insert, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    return resp;
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, $"ProcessCustomerReturnConfirmation {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);

                var siesaSr = _siesaServices.PlanoDevolucionesDeClientes(data);
                if (!siesaSr.Status)
                {
                    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                    new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, $"Error: PlanoDevolucionesDeClientes {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }

                respProcess = new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, "ProcessCustomerReturnConfirmation", trackingId, OperationType.Update, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                    new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, $"Error: PlanoDevolucionesDeClientes {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }

                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<CustomerReturnConfirmationDto>(data, $"Error: ProcessCustomerReturnConfirmation {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }

            return resp;
        }

        public PreparationConfirmationResponseDto ProcessPreparationConfirmation(PreparationConfirmationDto data)
        {
            var resp = new PreparationConfirmationResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessPreparationConfirmation", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }

                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, "ProcessPreparationConfirmation", trackingId, OperationType.Insert, null, data.CompanyId);

                var trackingPlanoCompromisosPedidoVentaId = Guid.NewGuid().ToString();
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"PlanoCompromisosPedidoVenta {message}", trackingPlanoCompromisosPedidoVentaId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_INPROCESS, data.CompanyId);
                resp = PlanoCompromisosPedidoVenta(data, trackingId, resp);
                if (resp.Codigo == 11)
                {
                    resp.Mensaje = resp.Mensaje;
                    new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"Error: PlanoCompromisosPedidoVenta {resp.Mensaje}", trackingPlanoCompromisosPedidoVentaId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"PlanoCompromisosPedidoVenta {message}", trackingPlanoCompromisosPedidoVentaId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);


                var trackingRemisionId = Guid.NewGuid().ToString();
                resp = PlanoRemisionarPedidosDeVenta(data, trackingId, resp);
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"RemisionarPedidosDeVenta {message}", trackingRemisionId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_INPROCESS, data.CompanyId);
                if (resp.Codigo == 11)
                {
                    resp.Mensaje = resp.Mensaje;
                    new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"Error: RemisionarPedidosDeVenta {resp.Mensaje}", trackingRemisionId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"RemisionarPedidosDeVenta {message}", trackingRemisionId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);
                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"Error: ProcessPreparationConfirmation {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }

            return resp;
        }

        public RecibirConfRecepcionResponseDto ProcessReceptionConfirmation(RecibirConfRecepcionDto data)
        {
            var resp = new RecibirConfRecepcionResponseDto
            {
                Codigo = 1,
                Mensaje = "OK",
            };

            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessReceptionConfirmation", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"ProcessReceptionConfirmation", trackingId, OperationType.Insert, null, data.CompanyId);

                var trackingPlanoLotesId = Guid.NewGuid().ToString();
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, "Process PlanoLotes", trackingPlanoLotesId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_INPROCESS, data.CompanyId);
                var respProcess = PlanoLotes(data, trackingPlanoLotesId, resp);
                if (respProcess.Codigo == 11)
                {
                    resp.Mensaje = respProcess.Mensaje;
                    resp.Codigo = respProcess.Codigo;
                    new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"Error: PlanoLotes {resp.Mensaje}", trackingPlanoLotesId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                }
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, "Process PlanoLotes", trackingPlanoLotesId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);


                var trackingPlanoEntradadeAlmacenId = Guid.NewGuid().ToString();
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"PlanoEntradadeAlmacen", trackingPlanoEntradadeAlmacenId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_INPROCESS, data.CompanyId);
                respProcess = PlanoEntradadeAlmacen(data, trackingPlanoEntradadeAlmacenId, resp);
                if (respProcess.Codigo == 11)
                {
                    resp.Mensaje = respProcess.Mensaje;
                    resp.Codigo = respProcess.Codigo;
                    new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"Error: PlanoEntradadeAlmacen {resp.Mensaje}", trackingPlanoEntradadeAlmacenId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                }
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, "Process PlanoEntradadeAlmacen", trackingPlanoEntradadeAlmacenId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"Error: ProcessReceptionConfirmation {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }
            new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"ProcessReceptionConfirmation", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);

            return resp;
        }

        public SupplierReturnConfirmationResponseDto ProcessSupplierReturnConfirmation(SupplierReturnConfirmationDto data)
        {
            var resp = new SupplierReturnConfirmationResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessSupplierReturnConfirmation", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }
                var respProcess = new EventlogHelper().ProcessEventLog<SupplierReturnConfirmationDto>(data, "ProcessSupplierReturnConfirmation", trackingId, OperationType.Insert, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    return resp;
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<SupplierReturnConfirmationDto>(data, $"ProcessSupplierReturnConfirmation {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);

                //TODO:call method siesa
                var siesaSr = _siesaServices.PlanoDevolucionesDeProveedor(data);
                if (!siesaSr.Status)
                {
                    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                    new EventlogHelper().ProcessEventLog<SupplierReturnConfirmationDto>(data, $"Error: PlanoTransferenciaInventario {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }

                respProcess = new EventlogHelper().ProcessEventLog<SupplierReturnConfirmationDto>(data, "ProcessSupplierReturnConfirmation", trackingId, OperationType.Update, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    return resp;
                }
                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<SupplierReturnConfirmationDto>(data, $"Error: ProcessSupplierReturnConfirmation {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }

            return resp;
        }

        public RecibirConfCargaCamionResponseDto ProcessConfCargaCamion(RecibirConfCargaCamionDto data)
        {
            var resp = new RecibirConfCargaCamionResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessConfCargaCamion", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }
                var respProcess = new EventlogHelper().ProcessEventLog<RecibirConfCargaCamionDto>(data, "RecibirConfCargaCamionResponse", trackingId, OperationType.Insert, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    return resp;
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<RecibirConfCargaCamionDto>(data, $"RecibirConfCargaCamionResponse {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);

                //TODO:call method siesa
                var siesaSr = _siesaServices.PlanoCuadredeFacturas(data);
                if (!siesaSr.Status)
                {
                    resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                    new EventlogHelper().ProcessEventLog<RecibirConfCargaCamionDto>(data, $"Error: PlanoCuadredeFacturas {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }
                respProcess = new EventlogHelper().ProcessEventLog<RecibirConfCargaCamionDto>(data, "RecibirConfCargaCamionResponse", trackingId, OperationType.Update, null, data.CompanyId);
                if (respProcess.Code == 11)
                {
                    resp.Mensaje = respProcess.Message;
                    new EventlogHelper().ProcessEventLog<RecibirConfCargaCamionDto>(data, $"Error: RecibirConfCargaCamionResponse {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    return resp;
                }
                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                return resp;
            }

            return resp;
        }

        private RecibirConfRecepcionResponseDto PlanoEntradadeAlmacen(RecibirConfRecepcionDto data, string trackingId, RecibirConfRecepcionResponseDto resp)
        {
            resp.Codigo = 1;
            resp.Mensaje = "OK";
            var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoEntradadeAlmacen", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
            if (!interfaceReceptorSr.Status)
            {
                resp.Mensaje = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                resp.Codigo = 11;
                return resp;
            }

            var interfaceReceptorLotesSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");

            var message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
            new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"PlanoEntradadeAlmacen {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);
            var siesaSr = _siesaServices.PlanoEntradadeAlmacen(data);
            if (!siesaSr.Status)
            {
                resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"Error: PlanoEntradadeAlmacen {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                resp.Codigo = 11;
                return resp;
            }
            return resp;
        }

        private RecibirConfRecepcionResponseDto PlanoLotes(RecibirConfRecepcionDto data, string trackingId, RecibirConfRecepcionResponseDto resp)
        {
            resp.Codigo = 1;
            resp.Mensaje = "OK";
            var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoLotes", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
            if (!interfaceReceptorSr.Status)
            {
                resp.Mensaje = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                resp.Codigo = 11;
                return resp;
            }

            var message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
            new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"PlanoLotes {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);
            var siesaSr = _siesaServices.PlanoLotes(data);
            if (!siesaSr.Status)
            {
                resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                new EventlogHelper().ProcessEventLog<RecibirConfRecepcionDto>(data, $"Error: PlanoLotes {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                resp.Codigo = 11;
                return resp;
            }
            return resp;
        }

        public OrdenFabricacionResponseDto ProcessConfOrdenFabricacion(OrdenFabricacionDto data)
        {
            var resp = new OrdenFabricacionResponseDto
            {
                Codigo = 11,
                Mensaje = string.Empty,
            };
            var trackingId = Guid.NewGuid().ToString();
            try
            {
                string message = string.Empty;
                var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("ProcessConfOrdenFabricacion", data.CompanyId);
                if (!interfaceReceptorSr.Status)
                {
                    message = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                    throw new Exception(message);
                }
                message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
                new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"OrdenFabricacionResponse {message}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_INPROCESS, data.CompanyId);

                if (data.ConfOrdenFabricacion.Accion.ToUpper() == "C")
                {
                    var trackingAgruparArtículosOrdenFabricacionId = Guid.NewGuid().ToString();
                    new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, "Plano Agrupar Artículos Orde Fabricacion", trackingAgruparArtículosOrdenFabricacionId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_INPROCESS, data.CompanyId);
                    var respProcess = PlanoAgruparArtículosOrdenFabricacion(data, trackingAgruparArtículosOrdenFabricacionId, resp);
                    if (respProcess.Codigo == 11)
                    {
                        resp.Mensaje = respProcess.Mensaje;
                        resp.Codigo = respProcess.Codigo;
                        new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"Plano Agrupar Artículos Orde Fabricacion {resp.Mensaje}", trackingAgruparArtículosOrdenFabricacionId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                    }
                    new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, "Plano Agrupar Artículos Orde Fabricacion", trackingAgruparArtículosOrdenFabricacionId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);
                }


                new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"OrdenFabricacionResponse {message}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_OK, data.CompanyId);
                resp.Codigo = 1;
                resp.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"Error: ProcessConfOrdenFabricacion {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }

            return resp;
        }

        private PreparationConfirmationResponseDto PlanoCompromisosPedidoVenta(PreparationConfirmationDto data, string trackingId, PreparationConfirmationResponseDto resp)
        {
            resp.Codigo = 1;
            resp.Mensaje = "OK";

            var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoCompromisosPedidoVenta", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
            if (!interfaceReceptorSr.Status)
            {
                resp.Mensaje = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                resp.Codigo = 11;
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"PlanoCompromisosPedidoVenta {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }
            var message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
            new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"PlanoCompromisosPedidoVenta {message}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);
            var siesaSr = _siesaServices.PlanoCompromisosPedidoVenta(data);
            if (!siesaSr.Status)
            {
                resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"Error: PlanoCompromisosPedidoVenta {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                resp.Codigo = 11;
                return resp;
            }
            return resp;
        }

        private PreparationConfirmationResponseDto PlanoRemisionarPedidosDeVenta(PreparationConfirmationDto data, string trackingId, PreparationConfirmationResponseDto resp)
        {
            resp.Codigo = 1;
            resp.Mensaje = "OK";

            var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoRemisionarPedidosDeVenta", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
            if (!interfaceReceptorSr.Status)
            {
                resp.Mensaje = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                resp.Codigo = 11;
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"PlanoRemisionarPedidosDeVenta {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }
            var message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
            new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"PlanoRemisionarPedidosDeVenta {message}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);
            var siesaSr = _siesaServices.PlanoRemisionarPedidosDeVenta(data);
            if (!siesaSr.Status)
            {
                resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                new EventlogHelper().ProcessEventLog<PreparationConfirmationDto>(data, $"Error: PlanoRemisionarPedidosDeVenta {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                resp.Codigo = 11;
                return resp;
            }
            return resp;
        }

        private OrdenFabricacionResponseDto PlanoAgruparArtículosOrdenFabricacion(OrdenFabricacionDto data, string trackingId, OrdenFabricacionResponseDto resp)
        {
            resp.Codigo = 1;
            resp.Mensaje = "OK";

            var interfaceReceptorSr = _interfaceReceptorRepository.GetKeyAndCompany("PlanoAgruparArtículosOrdenFabricacion", data.CompanyId, "RECEIVER_INTERNAL_FLOW");
            if (!interfaceReceptorSr.Status)
            {
                resp.Mensaje = interfaceReceptorSr.Errors.FirstOrDefault().ErrorDetail.Replace("{data.CompanyId}", data.CompanyId.ToString());
                resp.Codigo = 11;
                new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"PlanoAgruparArtículosOrdenFabricacion {resp.Mensaje}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                return resp;
            }
            var message = interfaceReceptorSr.Data.Description.Replace("{data.CompanyId}", data.CompanyId.ToString());
            new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"PlanoAgruparArtículosOrdenFabricacion {message}", trackingId, OperationType.Insert, Constants.LogIndex.TRAMSACTION_ACTIVE, data.CompanyId);
            var siesaSr = _siesaServices.PlanoAgruparArtículosOrdenFabricacion(data);
            if (!siesaSr.Status)
            {
                resp.Mensaje = siesaSr.Errors.FirstOrDefault()?.ErrorDetail;
                new EventlogHelper().ProcessEventLog<OrdenFabricacionDto>(data, $"Error: PlanoAgruparArtículosOrdenFabricacion {resp.Mensaje}", trackingId, OperationType.Update, Constants.LogIndex.TRAMSACTION_ERROR, data.CompanyId);
                resp.Codigo = 11;
                return resp;
            }
            return resp;
        }

    }

    public interface IReceptionServices
    {
        RecibirConfRecepcionResponseDto ProcessReceptionConfirmation(RecibirConfRecepcionDto data);
        PreparationConfirmationResponseDto ProcessPreparationConfirmation(PreparationConfirmationDto data);
        ConfirmationStockAdjustmentResponseDto ProcessConfirmationStockAdjustment(ConfirmationStockAdjustmentDto data);
        SupplierReturnConfirmationResponseDto ProcessSupplierReturnConfirmation(SupplierReturnConfirmationDto data);
        CustomerReturnConfirmationResponseDto ProcessCustomerReturnConfirmation(CustomerReturnConfirmationDto data);
        ChangeLogicalSituationResponseDto ProcessChangeLogicalSituation(ChangeLogicalSituationDto data);
        RecibirConfCargaCamionResponseDto ProcessConfCargaCamion(RecibirConfCargaCamionDto data);
        OrdenFabricacionResponseDto ProcessConfOrdenFabricacion(OrdenFabricacionDto data);
    }
}
