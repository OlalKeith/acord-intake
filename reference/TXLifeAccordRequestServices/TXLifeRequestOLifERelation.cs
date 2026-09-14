using System;
using System.Xml;

namespace BusinessServices.BAL.ACORD
{
	/// <summary>
	/// Summary description for TXLifeRequestOLifERelation.
	/// </summary>
	public class TXLifeRequestOLifERelation :Logger, IProcessXML
	{
		public TXLifeRequestOLifERelation()
		{
		}
		

        private string _relationNodeXML = "";
        public string RelationNodeXML
        {
            get { return _relationNodeXML; }
            set
            {

                _relationNodeXML = value.Trim();
            }
        }


		public bool ProcessXML(XmlNode node)
		{
            if (node == null)
                return false;

           
			try
			{
                XmlNodeList relationNodelist = node.SelectNodes("//OLifE/Relation");
                
                if (relationNodelist != null)
                {
                    
                    // get the xml for each node
                    foreach (XmlNode tempnode in relationNodelist)
                    {
                        _relationNodeXML += tempnode.OuterXml;
                    }
				    return true;
                }
			}
			catch(Exception)
			{
				//EventLog.WriteEntry (e.Source , e.Message + "\n" + "Stack = " + e.StackTrace, EventLogEntryType.Error);
				return false;
			}
            return false;
		}
	}
}
