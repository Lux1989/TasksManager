﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace TasksManager.Registration
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if ((requestContext == null) || (controllerType == null))
                    return null;

                return (Controller)StructureMapBootstrapper
                    .RegisteredContainer
                    .GetInstance(controllerType);
            }
            catch (StructureMapException ex)
            {
                throw new Exception("Registration failed with message: " + ex.Message);
            }
        }
    }
}