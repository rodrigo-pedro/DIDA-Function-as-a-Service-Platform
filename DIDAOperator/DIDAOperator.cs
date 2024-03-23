using DIDAWorker;
using Grpc.Net.Client;
using System;

namespace DIDAOperator
{
    public class WriteOperator : IDIDAOperator
    {
        IDIDAStorage _storageProxy;

        public void ConfigureStorage(IDIDAStorage storageProxy)
        {
            _storageProxy = storageProxy;
        }

        public string ProcessRecord(DIDAMetaRecord meta, string input, string previousOperatorOutput)
        {



            var version = _storageProxy.write(new DIDAWorker.DIDAWriteRequest
            {
                Id = input,
                Val = "francisca",
 
            });

            return "pancas";
        }
    }

    public class ReadOperator : IDIDAOperator
    {

        IDIDAStorage _storageProxy;

        public void ConfigureStorage(IDIDAStorage storageProxy)
        {
            _storageProxy = storageProxy;
        }

        public string ProcessRecord(DIDAMetaRecord meta, string input, string previousOperatorOutput)
        {
            var record = _storageProxy.read(new DIDAWorker.DIDAReadRequest
            {
                Id = input,
                Version = new DIDAWorker.DIDAVersion { ReplicaId = -1, VersionNumber = -1}
            });

            Console.WriteLine(record.Val);

            return record.Id;
        }
    }

    public class UpdateOperator : IDIDAOperator
    {
        IDIDAStorage _storageProxy;

        public void ConfigureStorage(IDIDAStorage storageProxy)
        {
            _storageProxy = storageProxy;
        }

        public string ProcessRecord(DIDAMetaRecord meta, string input, string previousOperatorOutput)
        {



            var version = _storageProxy.updateIfValueIs(new DIDAWorker.DIDAUpdateIfRequest
            {
                Id = input,
                Oldvalue = "francisca",
                Newvalue = "alentejano"

            });

            return "pancas";
        }
    }
}
