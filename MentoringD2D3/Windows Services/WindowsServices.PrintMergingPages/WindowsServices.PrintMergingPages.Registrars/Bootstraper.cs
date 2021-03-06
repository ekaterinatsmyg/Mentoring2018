﻿using System;
using System.IO;
using WindowsServices.PrintMergingPages.Service;
using Unity;
using Unity.Injection;

namespace WindowsServices.PrintMergingPages.Registrars
{
    public class Bootstraper
    {
        private static readonly IUnityContainer container = new UnityContainer();

        public static void BuildUnityContainer()
        {
            container
                .RegisterType<IDocumentFactory, ImagePdfFactory>()
                .RegisterType<IMergeDocument, ImagePdfMergeDocument>()
                .RegisterType<IMergePagesService, MergePagesService>(new InjectionConstructor(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output"), new ResolvedParameter<IDocumentFactory>()));
        }

        public static T Resolve<T>()
        {
            var ret = default(T);

            if (container.IsRegistered(typeof(T)))
            {
                ret = container.Resolve<T>();
            }

            return ret;
        }
    }
}
