using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VersoesGeraXml
/// </summary>
public class Versoes
{
    public int id { get; set; }
    public string Versao { get; set; }
    public DateTime Data { get; set; }
    public string Detalhes { get; set; }
    public string Path { get; set; }

	public Versoes()
	{

	}
}