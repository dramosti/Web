using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for belComissao
/// </summary>
public class belComissao
{
    public decimal VL_TOTAL { get; set; }
    public decimal VL_TOTPROD { get; set; }
    public decimal VL_COMISSAO { get; set; }
    public decimal VL_PERCOMISSAO { get; set; }
    public DateTime DT_DOC { get; set; }
    public string CD_EMPRESA { get; set; }
    public string CD_CLIFOR { get; set; }
    public string DS_TIPODOC { get; set; }
    public string CD_PEDIDO { get; set; }
    public string NM_CLIFOR { get; set; }
    public string NM_VEND { get; set; }
}