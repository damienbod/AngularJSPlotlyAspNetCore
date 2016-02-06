using System.Collections.Generic;
using AngularPlotlyAspNetCore.Models;

namespace AngularPlotlyAspNetCore
{
    public interface ISnakeDataRepository
    {
        List<Machine> GetMachines();

        OeeDataAverageAgg GetOeeForAll();

        List<OeeDataAverageAgg> GetOeeForMachines(List<string> machineNames);

        OeeDataProUnit GetLineDataForMachine(string machineName, string datapoint, string proYearMonthDay);

        void AddAllData();
    }
}
