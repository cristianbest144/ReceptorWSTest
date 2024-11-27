using BusinessLogicLayer.Services;
using System;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace ReceptorWS
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Configuración de la inyección de dependencias con Unity
            //var container = new UnityContainer();
            //RegisterTypes(container);
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            //// BusinessLogicLayer
            //container.RegisterType<IReceptionConfirmationServices, ReceptionConfirmationServices>();
            ////container.RegisterType<ISiesaServices, SiesaServices>();
            //// DataAccessLayer
            ////container.RegisterType<IReceptionConfirmationRepository, ReceptionConfirmationRepository>();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}