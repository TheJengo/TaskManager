using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using Core.DataAccess.Uow;
using Core.Entity;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>().InstancePerDependency();
            builder.RegisterType<UserTaskManager>().As<IUserTaskService>().InstancePerDependency();
            builder.RegisterType<TaskTypeManager>().As<ITaskTypeService>().InstancePerDependency();

            builder.RegisterType<EfUserDal>().As<IUserDal>().InstancePerDependency();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>().InstancePerDependency();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>().InstancePerDependency();
            builder.RegisterType<EfUserTaskDal>().As<IUserTaskDal>().InstancePerDependency();
            builder.RegisterType<EfTaskTypeDal>().As<ITaskTypeDal>().InstancePerDependency();
            builder.RegisterType<TaskManagerContext>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
