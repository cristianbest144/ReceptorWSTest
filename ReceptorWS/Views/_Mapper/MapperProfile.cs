using BusinessLogicLayer.Models;
using PresentationLayer.Views;
using System.Linq;

namespace Shared
{
    public partial class MapperProfile
    {
        public static RecibirConfRecepcionDto MapToReceptionConfirmationBusiness(RecibirConfRecepcion data, Company comp)
        {
            return new RecibirConfRecepcionDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfLineaRecep = new ConfLineaRecepDto
                {
                    ConfLineaRecepcion = data.ConfLineaRecep.ConfLineaRecepcion.Select(t => new ConfLineaRecepcionDto
                    {
                        Accion = t.Accion,
                        Articu = t.Articu,
                        Artpro = t.Artpro,
                        Artpv1 = t.Artpv1,
                        Artpv2 = t.Artpv2,
                        Artpvl = t.Artpvl,
                        Bulrec = t.Bulrec,
                        Canpes = t.Canpes,
                        Cantco = t.Cantco,
                        Canteo = t.Canteo,
                        Codlin = t.Codlin,
                        Feccad = t.Feccad,
                        Fecha = t.Fecha,
                        LoteFa = t.LoteFa,
                        Nument = t.Nument,
                        Pedext = t.Pedext,
                        Pedido = t.Pedido,
                        Sitlin = t.Sitlin,
                        Varia1 = t.Varia1,
                        Varia2 = t.Varia2,
                        Varlog = t.Varlog,
                    }).ToList()
                },
                ConfRecepcion = new ConfRecepcionDto
                {
                    Albara = data.ConfRecepcion.Albara,
                    Almace = data.ConfRecepcion.Almace,
                    Fecfin = data.ConfRecepcion.Fecfin,
                    Fecha = data.ConfRecepcion.Fecha,
                    Nument = data.ConfRecepcion.Nument,
                    Numpal = data.ConfRecepcion.Numpal,
                    Pedext = data.ConfRecepcion.Pedext,
                    Pedido = data.ConfRecepcion.Pedido,
                    Proext = data.ConfRecepcion.Proext,
                    Propie = data.ConfRecepcion.Propie,
                    Provee = data.ConfRecepcion.Provee,
                    Sitcab = data.ConfRecepcion.Sitcab,
                    Tipped = data.ConfRecepcion.Tipped,
                    Usuari = data.ConfRecepcion.Usuari
                },
            };
        }
        public static RecibirConfRecepcionResult MapToReceptionConfirmationPresentation(RecibirConfRecepcionResponseDto data)
        {
            return new RecibirConfRecepcionResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
        public static PreparationConfirmationDto MapToPreparationConfirmationBusiness(RecibirConfPreparacion data, Company comp)
        {
            return new PreparationConfirmationDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfPreparacion = new ConfPreparacionDto
                {
                    Accion = data.ConfPreparacion.Accion,
                    Agenci = data.ConfPreparacion.Agenci,
                    Almace = data.ConfPreparacion.Almace,
                    Client = data.ConfPreparacion.Client,
                    Cliext = data.ConfPreparacion.Cliext,
                    Divped = data.ConfPreparacion.Divped,
                    Fecha = data.ConfPreparacion.Fecha,
                    Fecser = data.ConfPreparacion.Fecser,
                    Fectra = data.ConfPreparacion.Fectra,
                    Pedext = data.ConfPreparacion.Pedext,
                    Pedido = data.ConfPreparacion.Pedido,
                    Propie = data.ConfPreparacion.Propie,
                    Sitped = data.ConfPreparacion.Sitped,
                    Tipped = data.ConfPreparacion.Tipped,
                    Totbul = data.ConfPreparacion.Totbul,
                    Usuari = data.ConfPreparacion.Usuari
                },
                ConfLineaPrep = new ConfLineaPrepDto
                {
                    LineasPreparacion = data.ConfLineaPrep.ConfLineaPreparacion.Select(t => new LineaPreparacionDto
                    {
                        Accion = t.Accion,
                        Almkit = t.Almkit,
                        Articu = t.Articu,
                        Artpro = t.Artpro,
                        Artpv1 = t.Artpv1,
                        Artpv2 = t.Artpv2,
                        Artpvl = t.Artpvl,
                        Bulrec = t.Bulrec,
                        Canped = t.Canped,
                        Canpes = t.Canpes,
                        Canrec = t.Canrec,
                        Causaf = t.Causaf,
                        Codlin = t.Codlin,
                        Divped = t.Divped,
                        Feccad = t.Feccad,
                        Fecha = t.Fecha,
                        Lote = t.Lote,
                        Pedext = t.Pedext,
                        Pedido = t.Pedido,
                        Pprkit = t.Pprkit,
                        Sitlin = t.Sitlin,
                        Varia1 = t.Varia1,
                        Varia2 = t.Varia2,
                        Varlog = t.Varlog
                    }).ToList()
                }
            };
        }
        public static RecibirConfPreparacionResult MapToPreparationConfirmationPresentation(PreparationConfirmationResponseDto data)
        {
            return new RecibirConfPreparacionResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
        public static ChangeLogicalSituationDto MapToChangeLogicalSituationBusiness(recibirConfCambioSituacionLogica data, Company comp)
        {
            return new ChangeLogicalSituationDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfCambioSituacionLogica = new ConfCambioSituacionLogicaDto()
                {
                    Accion = data.ConfCambioSituacionLogica.Accion,
                    Almace = data.ConfCambioSituacionLogica.Almace,
                    Articu = data.ConfCambioSituacionLogica.Articu,
                    Artpro = data.ConfCambioSituacionLogica.Artpro,
                    Artpv1 = data.ConfCambioSituacionLogica.Artpv1,
                    Artpv2 = data.ConfCambioSituacionLogica.Artpv2,
                    Artpvl = data.ConfCambioSituacionLogica.Artpvl,
                    Cantid = data.ConfCambioSituacionLogica.Cantid,
                    Coment = data.ConfCambioSituacionLogica.Coment,
                    Feccad = data.ConfCambioSituacionLogica.Feccad,
                    Feccam = data.ConfCambioSituacionLogica.Feccam,
                    Lotefa = data.ConfCambioSituacionLogica.Lotefa,
                    Propie = data.ConfCambioSituacionLogica.Propie,
                    Stnuev = data.ConfCambioSituacionLogica.Stnuev,
                    Stviej = data.ConfCambioSituacionLogica.Stviej,
                    Varia1 = data.ConfCambioSituacionLogica.Varia1,
                    Varia2 = data.ConfCambioSituacionLogica.Varia2,
                    Varlog = data.ConfCambioSituacionLogica.Varlog,
                },
            };
        }
        public static RecibirConfCambioSituacionLogicaResult MapToChangeLogicalSituationPresentation(ChangeLogicalSituationResponseDto data)
        {
            return new RecibirConfCambioSituacionLogicaResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
        public static ConfirmationStockAdjustmentDto MapToConfirmationStockAdjustmentBusiness(recibirConfRegularizacionStock data, Company comp)
        {
            return new ConfirmationStockAdjustmentDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfRegularizacionStock = new ConfRegularizacionStockDto
                {
                    Causal = data.ConfRegularizacionStock.Causal,
                    Varlog = data.ConfRegularizacionStock.Varlog,
                    Fecaju = data.ConfRegularizacionStock.Fecaju,
                    Propie = data.ConfRegularizacionStock.Propie,
                    Varia1 = data.ConfRegularizacionStock.Varia1,
                    Varia2 = data.ConfRegularizacionStock.Varia2,
                    Accion = data.ConfRegularizacionStock.Accion,
                    Almace = data.ConfRegularizacionStock.Almace,
                    Articu = data.ConfRegularizacionStock.Articu,
                    Artpro = data.ConfRegularizacionStock.Artpro,
                    Artpv1 = data.ConfRegularizacionStock.Artpv1,
                    Artpv2 = data.ConfRegularizacionStock.Artpv2,
                    Artpvl = data.ConfRegularizacionStock.Artpvl,
                    Bulrec = data.ConfRegularizacionStock.Bulrec,
                    Cantid = data.ConfRegularizacionStock.Cantid,
                    Codaju = data.ConfRegularizacionStock.Codaju,
                    Codmov = data.ConfRegularizacionStock.Codmov,
                    Feccad = data.ConfRegularizacionStock.Feccad,
                    Fecha = data.ConfRegularizacionStock.Fecha,
                    Paleta = data.ConfRegularizacionStock.Paleta,
                    Signoa = data.ConfRegularizacionStock.Signoa,
                    Sitlog = data.ConfRegularizacionStock.Sitlog,
                    Lotefa = data.ConfRegularizacionStock.Lotefa
                }
            };
        }
        public static RecibirConfRegularizacionStockResult MapToConfirmationStockAdjustmentPresentation(ConfirmationStockAdjustmentResponseDto data)
        {
            return new RecibirConfRegularizacionStockResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
        public static CustomerReturnConfirmationDto MapToCustomerReturnConfirmationBusiness(recibirConfDevolucionCliente data, Company comp)
        {
            return new CustomerReturnConfirmationDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfDevolucionCliente = new ConfDevolucionClienteDto
                {
                    Accion = data.ConfDevolucionCliente.Accion,
                    Numdoc = data.ConfDevolucionCliente.Numdoc,
                    Almace = data.ConfDevolucionCliente.Almace,
                    Cliente = data.ConfDevolucionCliente.Cliente,
                    Cliext = data.ConfDevolucionCliente.Cliext,
                    Codext = data.ConfDevolucionCliente.Codext,
                    Codigo = data.ConfDevolucionCliente.Codigo,
                    Fecdev = data.ConfDevolucionCliente.Fecdev,
                    Fecha = data.ConfDevolucionCliente.Fecha,
                    Propie = data.ConfDevolucionCliente.Propie,
                    Sitcab = data.ConfDevolucionCliente.Sitcab,
                    Totbul = data.ConfDevolucionCliente.Totbul,
                },
                ConfLineaDevolucionCliente = new ConfLineaDevolucionClienteDto
                {
                    LineasDevolucionCliente = data.ConfLineaDevolucionCliente.ConfLineaDevolucionCliente.Select(t => new LineaDevolucionClienteDto
                    {
                        Abonli = t.Abonli,
                        Accion = t.Accion,
                        Articu = t.Articu,
                        Artpro = t.Artpro,
                        Artpv1 = t.Artpv1,
                        Artpv2 = t.Artpv2,
                        Artpvl = t.Artpvl,
                        Canrea = t.Canrea,
                        Cantna = t.Cantna,
                        Cantot = t.Cantot,
                        Causad = t.Causad,
                        Codigo = t.Codigo,
                        Codlin = t.Codlin,
                        Feccad = t.Feccad,
                        Fecdev = t.Fecdev,
                        Fecha = t.Fecha,
                        Lotefa = t.Lotefa,
                        Numbul = t.Numbul,
                        Sitlin = t.Sitlin,
                        Varia1 = t.Varia1,
                        Varia2 = t.Varia2,
                        Varlog = t.Varlog,
                    }).ToList()
                }
            };
        }
        public static RecibirConfDevolucionClienteResult MapToCustomerReturnConfirmationPresentation(CustomerReturnConfirmationResponseDto data)
        {
            return new RecibirConfDevolucionClienteResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
        public static SupplierReturnConfirmationDto MapToSupplierReturnConfirmationBusiness(recibirConfDevolucionProveedor data, Company comp)
        {
            return new SupplierReturnConfirmationDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfDevolucionProveedor = new ConfDevolucionProveedorDto()
                {
                    Almace = data.ConfDevolucionProveedor.Almace,
                    Accion = data.ConfDevolucionProveedor.Accion,
                    Codext = data.ConfDevolucionProveedor.Codext,
                    Codigo = data.ConfDevolucionProveedor.Codigo,
                    Fecdev = data.ConfDevolucionProveedor.Fecdev,
                    Fecha = data.ConfDevolucionProveedor.Fecha,
                    Numdoc = data.ConfDevolucionProveedor.Numdoc,
                    Proext = data.ConfDevolucionProveedor.Proext,
                    Propie = data.ConfDevolucionProveedor.Propie,
                    Provee = data.ConfDevolucionProveedor.Provee,
                    Sitcab = data.ConfDevolucionProveedor.Sitcab,
                },
                ConfLineaDevolucionProveedor = new ConfLineaDevolucionProveedorDto
                {
                    LineasDevolucionProveedor = data.ConfLineaDevolucionProveedor.ConfLineaDevolucionProveedor.Select(t => new LineaDevolucionProveedorDto
                    {
                        Accion = t.Accion,
                        Aptnap = t.Aptnap,
                        Articu = t.Articu,
                        Artpro = t.Artpro,
                        Artpv1 = t.Artpv1,
                        Artpv2 = t.Artpv2,
                        Artpvl = t.Artpvl,
                        Canrea = t.Canrea,
                        Cantid = t.Cantid,
                        Causad = t.Causad,
                        Codext = t.Codext,
                        Codigo = t.Codigo,
                        Codlin = t.Codlin,
                        Feccad = t.Feccad,
                        Fecdev = t.Fecdev,
                        Fecha = t.Fecha,
                        Sitlin = t.Sitlin,
                        Varia1 = t.Varia1,
                        Varia2 = t.Varia2,
                        Varlog = t.Varlog,
                        LoteFa = t.LoteFa
                    }).ToList()
                }
            };
        }
        public static RecibirConfDevolucionProveedorResult MapToSupplierReturnConfirmationPresentation(SupplierReturnConfirmationResponseDto data)
        {
            return new RecibirConfDevolucionProveedorResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
        public static RecibirConfCargaCamionDto MapToRecibirConfCargaCamionBusiness(recibirConfCargaCamion data, Company comp)
        {
            return new RecibirConfCargaCamionDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfCargaCamion = new ConfCargaCamionDto()
                {
                    Codcot = data.ConfCargaCamion.Codcot,
                    Feccar = data.ConfCargaCamion.Feccar,
                    Matcot = data.ConfCargaCamion.Matcot,
                    Centro = data.ConfCargaCamion.Centro,
                    Vpl = data.ConfCargaCamion.Vpl,
                    Estcot = data.ConfCargaCamion.Estcot,
                    Accion = data.ConfCargaCamion.Accion,
                    Pesmax = data.ConfCargaCamion.Pesmax,
                    Volume = data.ConfCargaCamion.Volume,
                    Fecha = data.ConfCargaCamion.Fecha
                },
                ConfLineaCargaCamion = data.confLineaCargaCamion.ConfLineaCargaCamion.Select(t => new ConfLineaCargaCamionDto
                {
                    Codcot = t.Codcot,
                    Pedido = t.Pedido,
                    Pedext = t.Pedext,
                    Orden = t.Orden,
                    Accion = t.Accion,
                    Pesped = t.Pesped,
                    Cajaco = t.Cajaco,
                    Cajare = t.Cajare,
                    Bolsas = t.Bolsas,
                    Canast = t.Canast,
                    Listct = t.Listct,
                    Fecha = t.Fecha,
                }).ToList()
            };
        }
        public static RecibirConfCargaCamionResult MapToRecibirConfCargaCamionPresentation(RecibirConfCargaCamionResponseDto data)
        {
            return new RecibirConfCargaCamionResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }

        public static OrdenFabricacionDto MapToRecibirOrdenFabricacionBusiness(recibirOrdenFabricacion data, Company comp)
        {
            return new OrdenFabricacionDto
            {
                CompanyId = comp.CompanyId,
                SiesaCode = comp.SiesaCode,
                Message = new MessageDto
                {
                    MessageInfo = new MessageInfoDto
                    {
                        DateTime = data.Message.MessageInfo.dateTime,
                        OriginatorName = data.Message.MessageInfo.originatorName,
                    },
                    TrxId = data.Message.trxId
                },
                ConfOrdenFabricacion = new ConfOrdenFabricacionDto()
                {
                    Accion = data.confOrdenFabricacion.accion,
                    Articu = data.confOrdenFabricacion.articu,
                    Artpro = data.confOrdenFabricacion.artpro,
                    Artpv1 = data.confOrdenFabricacion.artpv1,
                    Artpv2 = data.confOrdenFabricacion.artpv2,
                    Artpvl = data.confOrdenFabricacion.artpvl,
                    Cantid = data.confOrdenFabricacion.cantid,
                    Codgof = data.confOrdenFabricacion.codgof,
                    Codofr = data.confOrdenFabricacion.codgof,
                    Feccad = data.confOrdenFabricacion.codgof,
                    Fecha = data.confOrdenFabricacion.fecha,
                    Fecreg = data.confOrdenFabricacion.fecreg,
                    Gofext = data.confOrdenFabricacion.gofext,
                    Lotefa = data.confOrdenFabricacion.lotefa,
                    Ofrext = data.confOrdenFabricacion.ofrext,
                    Varia1 = data.confOrdenFabricacion.varia1,
                    Varia2 = data.confOrdenFabricacion.varia2,
                    Varlog = data.confOrdenFabricacion.varlog,
                }
            };
        }
        public static RecibirConfOrdenFabricacionResult MapToRecibirOrdenFabricacionPresentation(OrdenFabricacionResponseDto data)
        {
            return new RecibirConfOrdenFabricacionResult
            {
                Codigo = data.Codigo,
                Mensaje = data.Mensaje,
            };
        }
    }
}
