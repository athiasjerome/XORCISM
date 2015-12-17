using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// http://social.msdn.microsoft.com/Forums/en-US/wcf/thread/27dfcf47-b2dd-4ef8-a49c-3113bf4e9497

namespace XManagerService 
{
    [ServiceContract]
    // [ServiceKnownType(typeof(Dictionary<string, object>))]
    // [ServiceKnownType(typeof(System.Int32[]))]
    public interface IService1
    {
        /*
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        */

        [OperationContract]
        int CreateSession(int serviceCategoryID, Guid userID, byte[] parameters,Decimal PeasCount,Decimal PeasValue);

        [OperationContract]
        bool CancelSession(int SessionID);

        [OperationContract]
        bool RestartSession(int SessionID);
    }

    /*
    // Use a data contract as illustrated in the sample below to add composite types to service operations
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
	[KnownType(typeof(int[]))]
    [KnownType(typeof(Dictionary<string, object>))]
    [KnownType(typeof(System.Int32[]))]
	public class SessionParameters
	{
		[DataMember]
		public object UsedForKnownTypeSerializationObject;

		[DataMember]
		public string StringValue
		{
			get;
			set;
		}

		[DataMember]
		public Dictionary<string, object> Parameters
		{
			get;
			set;
		}
	}
    */
}
