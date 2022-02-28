using System.Collections.Generic;

namespace MonitoringTrainingSessions.Lib.DB;

public interface IModel
{
    public void select(Dictionary<string, object?>? data = null);
    
    public List<object> selectAll(Dictionary<string, object?>? data = null, string? dopSql = null);
    
    public int count(Dictionary<string, object?>? data = null);

    public void setPropertiesValue(Dictionary<string, object> data);
    
    public string getTableName();
}