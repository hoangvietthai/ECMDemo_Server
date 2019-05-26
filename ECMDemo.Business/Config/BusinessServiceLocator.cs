using ECMDemo.Business.Handler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace ECMDemo.Business
{

    public class BusinessServiceLocator
    {
        // a map between contracts -> concrete implementation classes
        private IDictionary<Type, Type> servicesType;

        // a map containing references to concrete implementation already instantiated
        // (the service locator uses lazy instantiation).
        private IDictionary<Type, object> instantiatedServices;
        private static readonly BusinessServiceLocator _instance = new BusinessServiceLocator();
        public static BusinessServiceLocator Instance
        {
            get { return _instance; }
        }


        public void Init()
        {
            BuildServiceTypesMap();
        }
        internal BusinessServiceLocator()
        {
            this.servicesType = new Dictionary<Type, Type>();
            this.instantiatedServices = new Dictionary<Type, object>();
            this.BuildServiceTypesMap();
        }


        public T GetService<T>()
        {

            lock (instantiatedServices)
            {
                if (this.instantiatedServices.ContainsKey(typeof(T)))
                {
                    return (T)this.instantiatedServices[typeof(T)];
                }
                else
                {
                    try
                    {
                        // lazy initialization
                        try
                        {
                            // use reflection to invoke the service
                            ConstructorInfo constructor = servicesType[typeof(T)].GetConstructor(new Type[0]);
                            Debug.Assert(constructor != null, "Cannot find a suitable constructor for " + typeof(T));


                            T service = (T)constructor.Invoke(null);

                            // Add service
                            instantiatedServices.Add(typeof(T), service);

                            return service;

                        }
                        catch (KeyNotFoundException ex)
                        {
                            // LogService.Service.Error(ex);
                            throw new ApplicationException("The requested service is not registered | " + typeof(T));
                        }

                    }
                    catch (Exception ex)
                    {
                        //  LogService.Service.Error(ex);
                        throw new ApplicationException("Failed | " + typeof(T));
                    }
                }
            }

        }


        private void BuildServiceTypesMap()
        {
            // Log
            // servicesType.Add(typeof(IBookHandler), typeof(DbBookHandler));
            //
             servicesType.Add(typeof(IDbDirectoryHandler), typeof(DbDirectoryHandler));
            servicesType.Add(typeof(IDbDocumentHandler), typeof(DbDocumentHandler));
            servicesType.Add(typeof(IDbDocumentCategoryHandler), typeof(DbDocumentCategoryHandler));
            servicesType.Add(typeof(IDbDocumentGroupCateHandler), typeof(DbDocumentGroupCateHandler));
            servicesType.Add(typeof(IDbLoginHandler), typeof(DbLoginHandler));
            servicesType.Add(typeof(IDbUserHandler), typeof(DbUserHandler));
            servicesType.Add(typeof(IDbDepartmentHandler), typeof(DbDepartmentHandler));
            servicesType.Add(typeof(IDbSendDocumentHandler), typeof(DbSendDocumentHandler));
            servicesType.Add(typeof(IDbReceivedDocumenntHandler), typeof(DbReceivedDocumentHandler));
            servicesType.Add(typeof(IDbBusinessPartnerHandler), typeof(DbBusinessPartnerHandler));
            servicesType.Add(typeof(IDbDocumentStatusHandler), typeof(DbDocumentStatusHandler));
            servicesType.Add(typeof(IDbContactPersonHandler), typeof(DbContactPersonHandler));
            servicesType.Add(typeof(IDbInternalDocumentHandler), typeof(DbInternalDocumentHandler));
            servicesType.Add(typeof(IAuthenticate), typeof(Authenticate));
            servicesType.Add(typeof(IDbDocumentUnifyHandler), typeof(DbDocumentUnifyHandler));
            servicesType.Add(typeof(IDbTaskMessageHandler), typeof(DbTaskMessageHandler));
            servicesType.Add(typeof(IDbDocumentConfirmHandler), typeof(DbDocumentConfirmHandler));
            servicesType.Add(typeof(IDbDocumentProcessHandler), typeof(DbDocumentProcessHandler));
            servicesType.Add(typeof(IDbDocumentPerformHandler), typeof(DbDocumentPerformHandler));
        }
    }

}

