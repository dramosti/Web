using System;
using System.Collections.Generic;
using System.Text;
using HLP.Dados;
using HLP.Geral;

namespace HLP.Dados.Vendas
{

    public class MovitemPedidoDAO : BaseDAO
    {

        public MovitemPedidoDAO(Tabela oTabelas) : base(oTabelas)
        {
            if (!oTabelas.CodigoCliente.Equals("TORCETEX"))
                this.Tabela = "MOVITEM";
            else
                this.Tabela = "VI_MOVITEMWEB_TORCETEX";
            this.ChavePrimaria.Add("CD_EMPRESA");
            this.ChavePrimaria.Add("NR_LANC");
            this.Inicializar();            
        }

        protected override bool Exclusao()
        {
            return true;
        }

        protected override bool PrevalidaAlt()
        {
            return true;
        }

        protected override bool PrevalidaCad()
        {
            return true;
        }

        protected override bool PosvalidaCad()
        {
            return true;
        }

        protected override bool Valid(ITela oCampo)
        {
            return true;
        }

        protected override bool When(ITela oCampo)
        {
            return true;
        }

        protected override void GerarNovoRegistro()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }

}
