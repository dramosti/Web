using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CalculaSubstTrib
/// </summary>
public class CalculaSubstTrib
{
    private decimal _TOT_PROD_ICMS;

    public decimal TOT_PROD_ICMS
    {
        get { return _TOT_PROD_ICMS; }
        set { _TOT_PROD_ICMS = value; }
    }
    private decimal _ALIQ_ICMS;

    public decimal ALIQ_ICMS
    {
        get { return _ALIQ_ICMS; }
        set { _ALIQ_ICMS = value; }
    }
    private decimal _PERC_REDUCAO;

    public decimal PERC_REDUCAO
    {
        get { return _PERC_REDUCAO; }
        set { _PERC_REDUCAO = value; }
    }
    private decimal _TOT_PROD_REDUZIDO;

    public decimal TOT_PROD_REDUZIDO
    {
        get { return _TOT_PROD_REDUZIDO; }
        set { _TOT_PROD_REDUZIDO = value; }
    }
    private bool _REDUZ_IPI_DO_ICMS;

    public bool REDUZ_IPI_DO_ICMS
    {
        get { return _REDUZ_IPI_DO_ICMS; }
        set { _REDUZ_IPI_DO_ICMS = value; }
    }
    private bool _REDUZ_BASE_DO_ICMS_A_SER_SUBTRAIDO;
    /// <summary>
    /// ICMS A SER SUBTRAÍDO = TOTAL DO PRODUTO SEM IPI * % DE REDUÇÃO * ALÍQUOTA DE ICMS --> ICMS NORMAL
    /// </summary>
    public bool REDUZ_BASE_DO_ICMS_A_SER_SUBTRAIDO
    {
        get { return _REDUZ_BASE_DO_ICMS_A_SER_SUBTRAIDO; }
        set { _REDUZ_BASE_DO_ICMS_A_SER_SUBTRAIDO = value; }
    }
    private bool _REDUZ_BASE_ICMS_DE_SUBST;

    public bool REDUZ_BASE_ICMS_DE_SUBST
    {
        get { return _REDUZ_BASE_ICMS_DE_SUBST; }
        set { _REDUZ_BASE_ICMS_DE_SUBST = value; }
    }
    private decimal _ICMS_A_SER_SUBTRAIDO;

    public decimal ICMS_A_SER_SUBTRAIDO
    {
        get { return _ICMS_A_SER_SUBTRAIDO; }
        set { _ICMS_A_SER_SUBTRAIDO = value; }
    }
    private decimal _TOTAL_DO_IPI;

    public decimal TOTAL_DO_IPI
    {
        get { return _TOTAL_DO_IPI; }
        set { _TOTAL_DO_IPI = value; }
    }
    private decimal _TOTAL_DO_IPI_REDUZIDO;

    public decimal TOTAL_DO_IPI_REDUZIDO
    {
        get { return _TOTAL_DO_IPI_REDUZIDO; }
        set { _TOTAL_DO_IPI_REDUZIDO = value; }
    }
    private decimal _TOTAL_DO_FRETE;

    public decimal TOTAL_DO_FRETE
    {
        get { return _TOTAL_DO_FRETE; }
        set { _TOTAL_DO_FRETE = value; }
    }
    private decimal _TOTAL_DE_OUTRAS;

    public decimal TOTAL_DE_OUTRAS
    {
        get { return _TOTAL_DE_OUTRAS; }
        set { _TOTAL_DE_OUTRAS = value; }
    }
    private decimal _TOTAL_DO_SEGURO;

    public decimal TOTAL_DO_SEGURO
    {
        get { return _TOTAL_DO_SEGURO; }
        set { _TOTAL_DO_SEGURO = value; }
    }
    private decimal _TOTAL_DO_PROD_COM_IPI;
    /// <summary>
    /// TOTAL DO PRODUTO COM IPI, COM FRETE, COM SEGURO E COM OUTRAS
    /// </summary>
    public decimal TOTAL_DO_PROD_COM_IPI
    {
        get { return _TOTAL_DO_PROD_COM_IPI; }
        set { _TOTAL_DO_PROD_COM_IPI = value; }
    }
    private decimal _ALIQ_DE_SUBST;

    public decimal ALIQ_DE_SUBST
    {
        get { return _ALIQ_DE_SUBST; }
        set { _ALIQ_DE_SUBST = value; }
    }
    private decimal _TOT_PROD_COM_IPI;

    public decimal TOT_PROD_COM_IPI
    {
        get { return _TOT_PROD_COM_IPI; }
        set { _TOT_PROD_COM_IPI = value; }
    }
    private decimal _PRECO_VAREJO;

    public decimal PRECO_VAREJO
    {
        get { return _PRECO_VAREJO; }
        set { _PRECO_VAREJO = value; }
    }
    private decimal _ALIQ_INTERNA;

    public decimal ALIQ_INTERNA
    {
        get { return _ALIQ_INTERNA; }
        set { _ALIQ_INTERNA = value; }
    }
    private decimal _ICMS_INTERNO;

    public decimal ICMS_INTERNO
    {
        get { return _ICMS_INTERNO; }
        set { _ICMS_INTERNO = value; }
    }
    private decimal _ICMS_SUBSTITUIDO;

    public decimal ICMS_SUBSTITUIDO
    {
        get { return _ICMS_SUBSTITUIDO; }
        set { _ICMS_SUBSTITUIDO = value; }
    }

    public CalculaSubstTrib()
    {
    }

    public decimal CalculaSubstitucaoTrubitariaProduto(int icodProduto) 
    {
        try
        {
            //Carregar Parametros
           
            return 0;
        }
        catch (Exception ex)
        {           
            throw ex;
        }
    }

}