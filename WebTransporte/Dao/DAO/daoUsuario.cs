using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;
using System.Web;
using System.IO;

namespace DAO
{
    public class daoUsuario
    {
        public string NM_USER_EMP { get; set; }
        public string NM_SENHA { get; set; }
        public string NM_CLIFOR { get; set; }
        public string CD_CGC { get; set; }
        public string CD_CLIFOR { get; set; }

        /* IMAGEM DA EMPRESA */
        public string IMG_EMP { get; set; }

        /* DADOS EMPRESA */
        public string CD_EMPRESA { get; set; } //Setado no método setImageEmpresa
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string NM_EMPRESA { get; set; }
        public string NM_FANT { get; set; }
        public string LOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string NUMERO { get; set; }
        public string BAIRRO { get; set; }
        public string MUNICIPIO { get; set; }
        public string CEP { get; set; }
        public string UF { get; set; }
        public string FONE { get; set; }





        public daoUsuario RetornaTeste(string str, string str2)
        {
            daoUsuario d = new daoUsuario();
            d.NM_USER_EMP = str;
            d.NM_SENHA = str2;
            return d;
        }

        public bool ValidaUser_Senha()
        {
            try
            {
                bool bValida = false;

                using (FbConnection Conn = new FbConnection(daoStatic.sConn))
                {
                    if (this.UserExists())
                    {
                        StringBuilder sQueryConc = new StringBuilder();
                        sQueryConc.Append("SELECT {0} ");
                        sQueryConc.Append("C.nm_clifor, C.cd_cgc, C.cd_clifor, {0} ");
                        sQueryConc.Append("coalesce(C.nm_senha_web,0) {0} ");
                        sQueryConc.Append("FROM CLIFOR C ");
                        sQueryConc.Append("WHERE C.cd_cgc = @cnpj and C.nm_senha_web = @senha");

                        string sQuery = string.Format(sQueryConc.ToString(), Environment.NewLine);

                        using (FbCommand cmd = new FbCommand(sQuery, Conn))
                        {
                            Conn.Open();
                            cmd.Parameters.AddWithValue("@cnpj", this.CD_CGC);
                            cmd.Parameters.AddWithValue("@senha", this.NM_SENHA);
                            FbDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                bValida = true;
                                this.CD_CGC = dr["cd_cgc"].ToString();
                                this.CD_CLIFOR = dr["cd_clifor"].ToString();
                                this.NM_CLIFOR = dr["nm_clifor"].ToString();
                                daoStatic.EMP_NOME_CGC = dr["nm_clifor"].ToString() + " - " + dr["cd_cgc"].ToString();
                            }
                        }
                    }
                }

                if (bValida)
                {
                    setImageEmpresa();
                    setDadosEmpresa();
                }

                return bValida;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setImageEmpresa()
        {
            try
            {
                using (FbConnection Conn = new FbConnection(daoStatic.sConn))
                {
                    string sQuery = "SELECT count(distinct(c.cd_empresa)) total FROM conhecim c WHERE c.cd_empresa = '001' AND cd_clifor = @cdCli";

                    using (FbCommand cmd = new FbCommand(sQuery, Conn))
                    {
                        Conn.Open();
                        cmd.Parameters.AddWithValue("@cdCli", this.CD_CLIFOR);

                        FbDataReader dr = cmd.ExecuteReader();

                        dr.Read();
                        if (dr["total"].ToString() == "1")
                        {
                            this.IMG_EMP = "imagem/LogoGca.png";
                            this.CD_EMPRESA = "001";
                        }
                        else
                        {
                            this.IMG_EMP = "imagem/LogoGca2.png";
                            this.CD_EMPRESA = "002";
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void setDadosEmpresa()
        {
            try
            {
                using (FbConnection Conn = new FbConnection(daoStatic.sConn))
                {
                    string query = "select  coalesce (empresa.cd_cgc,'')CNPJ," +
                                    "coalesce (empresa.cd_insest,'') IE," +
                                    "empresa.nm_empresa xNome," +
                                    "empresa.nm_guerra xFant," +
                                    "coalesce (empresa.ds_endnor,'') xLgr," +
                                    "coalesce (empresa.ds_endcomp,'')xCpl," +
                                    "coalesce (empresa.nr_end,'')nro," +
                                    "coalesce (empresa.nm_bairronor,'')xBairro," +
                                    "coalesce (empresa.nm_cidnor,'')xMun," +
                                    "coalesce (empresa.cd_cepnor,'')CEP," +
                                    "coalesce (empresa.cd_ufnor,'') UF," +
                                    "coalesce (empresa.cd_fonenor,'')fone" +
                                    " from empresa " +
                                    "left join cidades on empresa.nm_cidnor = cidades.nm_cidnor and cidades.cd_ufnor = empresa.cd_ufnor" +
                                    " where empresa.cd_empresa = @cod";

                    using (FbCommand com = new FbCommand(query, Conn))
                    {
                        Conn.Open();
                        com.Parameters.AddWithValue("@cod", this.CD_EMPRESA);

                        FbDataReader dr = com.ExecuteReader();

                        dr.Read();
                        this.CNPJ = dr["CNPJ"].ToString();
                        this.IE = dr["IE"].ToString();
                        this.NM_EMPRESA = dr["xNome"].ToString();
                        this.NM_FANT = dr["xFant"].ToString();
                        this.LOGRADOURO = dr["xLgr"].ToString();
                        this.COMPLEMENTO = dr["xCpl"].ToString();
                        this.NUMERO = dr["nro"].ToString();
                        this.BAIRRO = dr["xBairro"].ToString();
                        this.MUNICIPIO = dr["xMun"].ToString();
                        this.CEP = dr["CEP"].ToString();
                        this.UF = dr["UF"].ToString();
                        this.FONE = dr["fone"].ToString();

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool UserExists()
        {
            try
            {
                int iTotal = 0;
                using (FbConnection Conn = new FbConnection(daoStatic.sConn))
                {
                    StringBuilder sQueryConc = new StringBuilder();
                    sQueryConc.Append("SELECT count(C.cd_clifor) total FROM CLIFOR C ");
                    sQueryConc.Append("WHERE C.cd_cgc = @cnpj and C.nm_senha_web = @senha");

                    using (FbCommand cmd = new FbCommand(sQueryConc.ToString(), Conn))
                    {
                        Conn.Open();
                        cmd.Parameters.AddWithValue("@cnpj", this.CD_CGC);
                        cmd.Parameters.AddWithValue("@senha", this.NM_SENHA);
                        iTotal = Convert.ToInt32(cmd.ExecuteScalar());
                        Conn.Close();
                    }
                    return (iTotal == 0 ? false : true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
