﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galant.DataEntity;
using System.Reflection;

namespace GLTService
{
    // NOTE: If you change the interface name "IServiceAPI" here, you must also update the reference to "IService1" in Web.config.
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypesProvider))] 
    public interface IServiceAPI
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        BaseData DoRequest(BaseData composite, Entity staff, string OperationType);

        // TODO: Add your service operations here
    }

    static class KnownTypesProvider
    {
        static Type[] GetKnownTypes(ICustomAttributeProvider knownTypeAttributeTarget)
        {




            Type contractType = (Type)knownTypeAttributeTarget;


            return contractType.GetGenericArguments();


        }

    }

    
}
