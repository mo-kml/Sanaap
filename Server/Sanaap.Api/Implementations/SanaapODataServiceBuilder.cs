using Bit.OData.Contracts;
using System.Reflection;
using System.Web.OData.Builder;

namespace Sanaap.Api.Implementations
{
    public class SanaapODataServiceBuilder : IODataServiceBuilder
    {
        public virtual IAutoODataModelBuilder AutoEdmBuilder { get; set; }

        public virtual void BuildModel(ODataModelBuilder edmModelBuilder)
        {
            AutoEdmBuilder.AutoBuildODatModelFromAssembly(typeof(SanaapODataServiceBuilder).GetTypeInfo().Assembly, edmModelBuilder);
        }

        public virtual string GetODataRoute()
        {
            return "Sanaap";
        }
    }
}