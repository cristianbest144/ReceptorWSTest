using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using PresentationLayer.Views;
using Shared;
using System;
using System.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;
using Unity;
using Message = PresentationLayer.Views.Message;

namespace ReceptorWS
{
    /// <summary>
    /// Summary description for Receptor
    /// </summary>
    [WebService(Name = "ReceptorWS", Namespace = "")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Receptor : WebService
    {
        // Crear una instancia de MapperProfile
        private readonly IReceptionServices _process;
        private Company company = null;
        public Receptor()
        {
            // Crea una instancia de la fábrica de contenedores Unity
            var container = new UnityContainer();

            // Registra las dependencias en el contenedor
            container.RegisterType<IReceptionServices, ReceptionServices>();

            // Resuelve las dependencias y asigna la instancia al campo _receptionServices
            _process = container.Resolve<IReceptionServices>();

            company = new Company() {
                CompanyId = int.Parse(ConfigurationSettings.AppSettings[Constants.COMPANYID]),
                SiesaCode = int.Parse(ConfigurationSettings.AppSettings[Constants.SIESACODE]),
                Other1 = ConfigurationSettings.AppSettings[Constants.OTHER1],
                Other2 = ConfigurationSettings.AppSettings[Constants.OTHER2],
                Other3 = ConfigurationSettings.AppSettings[Constants.OTHER3]
            };
            Console.WriteLine(Constants.CONNECTIONSTRINGNAME);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfRecepcion")]
        public RecibirConfRecepcionResult recibirConfRecepcion(Message message, ConfRecepcion confRecepcion, ConfLineaRecep confLineaRecep)
        {
            var resp = new RecibirConfRecepcionResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToReceptionConfirmationBusiness(new RecibirConfRecepcion
                {
                    Message = message,
                    ConfRecepcion = confRecepcion,
                    ConfLineaRecep = confLineaRecep,
                }, company);
                resp = _process.ProcessReceptionConfirmation(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo=11;
            }

            return MapperProfile.MapToReceptionConfirmationPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfPreparacion")]
        public RecibirConfPreparacionResult recibirConfPreparacion(Message message,ConfPreparacion confPreparacion,ConfLineaPrep confLineaPrep)
        {

            var resp = new PreparationConfirmationResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToPreparationConfirmationBusiness(new RecibirConfPreparacion
                {
                    Message = message,
                    ConfPreparacion = confPreparacion,
                    ConfLineaPrep = confLineaPrep,
                }, company);
                resp = _process.ProcessPreparationConfirmation(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToPreparationConfirmationPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfRegularizacionStock")]
        public RecibirConfRegularizacionStockResult recibirConfRegularizacionStock(Message message,ConfRegularizacionStock confRegularizacionStock)
        {

            var resp = new ConfirmationStockAdjustmentResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToConfirmationStockAdjustmentBusiness(new recibirConfRegularizacionStock
                {
                    Message = message,
                    ConfRegularizacionStock = confRegularizacionStock
                }, company);
                resp = _process.ProcessConfirmationStockAdjustment(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToConfirmationStockAdjustmentPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfDevolucionProveedor")]
        public RecibirConfDevolucionProveedorResult recibirConfDevolucionProveedor(Message message, ConfDevolucionProveedor confDevolucionProveedor, confLineaDevolucionProveedor confLineaDevolucionProveedor)
        {
            var resp = new SupplierReturnConfirmationResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToSupplierReturnConfirmationBusiness(new recibirConfDevolucionProveedor
                {
                    Message = message,
                    ConfDevolucionProveedor = confDevolucionProveedor,
                    ConfLineaDevolucionProveedor= confLineaDevolucionProveedor
                }, company);
                resp = _process.ProcessSupplierReturnConfirmation(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToSupplierReturnConfirmationPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfDevolucionCliente")]
        public RecibirConfDevolucionClienteResult recibirConfDevolucionCliente(Message message, ConfDevolucionCliente confDevolucionCliente,  confLineaDevolucionCliente confLineaDevolucionCliente)
        {
            var resp = new CustomerReturnConfirmationResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToCustomerReturnConfirmationBusiness(new recibirConfDevolucionCliente
                {
                    Message = message,
                    ConfDevolucionCliente = confDevolucionCliente,
                    ConfLineaDevolucionCliente = confLineaDevolucionCliente
                }, company);
                resp = _process.ProcessCustomerReturnConfirmation(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToCustomerReturnConfirmationPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfCambioSituacionLogica")]
        public RecibirConfCambioSituacionLogicaResult recibirConfCambioSituacionLogica(Message message, ConfCambioSituacionLogica confCambioSituacionLogica)
        {
            var resp = new ChangeLogicalSituationResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToChangeLogicalSituationBusiness(new recibirConfCambioSituacionLogica
                {
                    Message = message,
                    ConfCambioSituacionLogica = confCambioSituacionLogica
                }, company);
                resp = _process.ProcessChangeLogicalSituation(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToChangeLogicalSituationPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfCargaCamion")]
        public RecibirConfCargaCamionResult recibirConfCargaCamion(Message message, ConfCargaCamion confCargaCamion, confLineaCargaCamion confLineaCargaCamion)
        {
            var resp = new RecibirConfCargaCamionResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToRecibirConfCargaCamionBusiness(new recibirConfCargaCamion
                {
                    Message = message,
                    ConfCargaCamion = confCargaCamion,
                    confLineaCargaCamion = confLineaCargaCamion
                }, company);
                resp = _process.ProcessConfCargaCamion(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToRecibirConfCargaCamionPresentation(resp);
        }

        [WebMethod]
        [SoapDocumentMethod(Action = "recibirConfOrdenFabricacion")]
        public RecibirConfOrdenFabricacionResult recibirOrdenFabricacion(Message message, OrdenFabricacion confOrdenFabricacion)
        {
            var resp = new OrdenFabricacionResponseDto();

            try
            {
                var businessObject = MapperProfile.MapToRecibirOrdenFabricacionBusiness(new recibirOrdenFabricacion
                {
                    Message = message,
                    confOrdenFabricacion = confOrdenFabricacion
                }, company);
                resp = _process.ProcessConfOrdenFabricacion(businessObject);
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.ToString();
                resp.Codigo = 11;
            }

            return MapperProfile.MapToRecibirOrdenFabricacionPresentation(resp);
        }
    }
}
