﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoBr.Dominio;

namespace BoletoBr.Bancos.Brasil
{
    public class EscritorRemessaCnab400BancoDoBrasil : IEscritorArquivoRemessa
    {
        public List<string> EscreverArquivo(List<Boleto> boletosEscrever)
        {
            throw new NotImplementedException();
        }

        public void ValidarArquivoRemessa(Cedente cedente, List<Boleto> boletos, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        public string EscreverHeader(Boleto boleto, int numeroRemessa, int numeroRegistro)
        {
            if (boleto == null)
                throw new Exception("Não há boleto para geração do HEADER");

            if (numeroRemessa == 0)
                throw new Exception("Sequencial da remessa não foi informado na geração do HEADER.");

            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do HEADER.");

            var header = new string(' ', 400);
            try
            {
                header = header.PreencherValorNaLinha(1, 1, "0");
                header = header.PreencherValorNaLinha(2, 2, "1");

                if (boleto.Remessa.Ambiente == Remessa.EnumTipoAmbiemte.Producao)
                    header = header.PreencherValorNaLinha(3, 9, "REMESSA".PadRight(7, ' '));
                else
                    header = header.PreencherValorNaLinha(3, 9, "TESTE".PadRight(7, ' '));
                header = header.PreencherValorNaLinha(10, 11, "01");
                header = header.PreencherValorNaLinha(12, 19, "COBRANCA".PadRight(8, ' '));
                header = header.PreencherValorNaLinha(20, 26, string.Empty.PadRight(7, ' '));
                header = header.PreencherValorNaLinha(27, 30,
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'));
                header = header.PreencherValorNaLinha(31, 31, boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);
                header = header.PreencherValorNaLinha(32, 39,
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(8, '0'));
                header = header.PreencherValorNaLinha(40, 40, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                header = header.PreencherValorNaLinha(41, 46, string.Empty.PadRight(6, '0'));
                header = header.PreencherValorNaLinha(47, 76, boleto.CedenteBoleto.Nome.ToUpper().PadRight(30, ' '));
                header = header.PreencherValorNaLinha(77, 94, "001BANCODOBRASIL".PadRight(18, ' '));
                header = header.PreencherValorNaLinha(95, 100, DateTime.Now.ToString("ddMMyy"));
                header = header.PreencherValorNaLinha(101, 107, numeroRemessa.ToString().PadRight(7, '0'));
                header = header.PreencherValorNaLinha(108, 129, string.Empty.PadRight(22, ' '));
                header = header.PreencherValorNaLinha(130, 136, boleto.CedenteBoleto.Convenio.PadLeft(7, '0'));
                header = header.PreencherValorNaLinha(137, 394, string.Empty.PadRight(258, ' '));
                header = header.PreencherValorNaLinha(395, 400, "000001");

                return header;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("BoletoBr{0}Falha na geração do HEADER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverDetalhe(Boleto boleto, int numeroRegistro)
        {
            if (boleto == null)
                throw new Exception("Não há boleto para geração do DETALHE");

            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do DETALHE.");

            #region Variáveis

            //string codigoCedente = "0" + boleto.CarteiraCobranca.Codigo.PadLeft(2, '0') +
            //                       boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(5, '0') +
            //                       boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(7, '0') +
            //                       boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta;
            var enderecoSacado = string.Empty;
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.TipoLogradouro;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Logradouro;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Numero;
            enderecoSacado += " ";
            enderecoSacado += boleto.SacadoBoleto.EnderecoSacado.Complemento;

            #endregion

            if (enderecoSacado.Length > 40)
                throw new Exception("Endereço do sacado excedeu o limite permitido.");

            var detalhe = new string(' ', 400);
            try
            {
                detalhe = detalhe.PreencherValorNaLinha(1, 1, "7");
                detalhe = detalhe.PreencherValorNaLinha(2, 3,
                    boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02");
                detalhe = detalhe.PreencherValorNaLinha(4, 17,
                    boleto.CedenteBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(18, 21,
                    boleto.CedenteBoleto.ContaBancariaCedente.Agencia.PadLeft(4, '0'));
                detalhe = detalhe.PreencherValorNaLinha(22, 22, boleto.CedenteBoleto.ContaBancariaCedente.DigitoAgencia);
                detalhe = detalhe.PreencherValorNaLinha(23, 30,
                    boleto.CedenteBoleto.ContaBancariaCedente.Conta.PadLeft(8, '0'));
                detalhe = detalhe.PreencherValorNaLinha(31, 31, boleto.CedenteBoleto.ContaBancariaCedente.DigitoConta);
                detalhe = detalhe.PreencherValorNaLinha(32, 38, boleto.CedenteBoleto.Convenio.PadLeft(7, '0'));

                const string doc = "DOC";
                var seuNumero = doc + boleto.NossoNumeroFormatado.PadRight(25 - doc.Length, ' ');

                detalhe = detalhe.PreencherValorNaLinha(39, 63, seuNumero);

                if (boleto.CarteiraCobranca.Codigo == "11" || boleto.CarteiraCobranca.Codigo == "31" ||
                    boleto.CarteiraCobranca.Codigo == "51")
                    detalhe = detalhe.PreencherValorNaLinha(64, 80, string.Empty.PadRight(17, '0'));
                else if (boleto.CarteiraCobranca.Codigo == "12" || boleto.CarteiraCobranca.Codigo == "15" ||
                         boleto.CarteiraCobranca.Codigo == "17")
                    detalhe = detalhe.PreencherValorNaLinha(64, 80,
                        boleto.CedenteBoleto.Convenio.PadLeft(7, '0') +
                        boleto.SequencialNossoNumero.PadLeft(10, '0'));
                else
                    detalhe = detalhe.PreencherValorNaLinha(64, 80,
                        boleto.CedenteBoleto.Convenio.PadLeft(7, '0') +
                        boleto.SequencialNossoNumero.PadLeft(10, '0'));

                detalhe = detalhe.PreencherValorNaLinha(81, 82, "00");
                detalhe = detalhe.PreencherValorNaLinha(83, 84, "00");
                detalhe = detalhe.PreencherValorNaLinha(85, 87, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(88, 88, " "); // Indicador de Observações/Mensagem posição 352-391
                detalhe = detalhe.PreencherValorNaLinha(89, 91, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(92, 94, boleto.CarteiraCobranca.Variacao); /* VARIAÇÃO DA CARTEIRA */
                detalhe = detalhe.PreencherValorNaLinha(95, 95, "0");
                detalhe = detalhe.PreencherValorNaLinha(96, 101, "000000");

                if (boleto.CarteiraCobranca.Codigo == "11" || boleto.CarteiraCobranca.Codigo == "17")
                {
                    if (boleto.CarteiraCobranca.Variacao == "DESCONTADA")
                    detalhe = detalhe.PreencherValorNaLinha(102, 106, "04DSC");

                    if (boleto.CarteiraCobranca.Variacao == "BBVENDOR")   
                    detalhe = detalhe.PreencherValorNaLinha(102, 106, "08VDR");

                    if (boleto.CarteiraCobranca.Variacao == "VINCULADA")   
                    detalhe = detalhe.PreencherValorNaLinha(102, 106, "02VIN");

                    if (boleto.CarteiraCobranca.Variacao == "SIMPLES")   
                    detalhe = detalhe.PreencherValorNaLinha(102, 106, string.Empty.PadRight(5, ' '));
                }

                if (boleto.CarteiraCobranca.Codigo == "12" || boleto.CarteiraCobranca.Codigo == "31" ||
                    boleto.CarteiraCobranca.Codigo == "51")
                {
                    detalhe = detalhe.PreencherValorNaLinha(102, 106, string.Empty.PadRight(5, ' '));
                }

                detalhe = detalhe.PreencherValorNaLinha(107, 108, boleto.CarteiraCobranca.Codigo.PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(109, 110, boleto.CodigoOcorrenciaRemessa.Codigo.ToString());
                detalhe = detalhe.PreencherValorNaLinha(111, 120,
                    boleto.SequencialNossoNumero.ToString().PadLeft(10, '0'));
                detalhe = detalhe.PreencherValorNaLinha(121, 126, boleto.DataVencimento.ToString("ddMMyy"));

                #region VALOR BOLETO

                var valorBoleto = string.Empty;

                if (boleto.ValorBoleto.ToString("f").Contains('.') && boleto.ValorBoleto.ToString("f").Contains(','))
                {
                    valorBoleto = boleto.ValorBoleto.ToString("f").Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.ValorBoleto.ToString("f").Contains('.'))
                {
                    valorBoleto = boleto.ValorBoleto.ToString("f").Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.ValorBoleto.ToString("f").Contains(','))
                {
                    valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(127, 139, valorBoleto.ToString().PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(127, 139, boleto.ValorBoleto.ToString("f").Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(140, 142, "001");
                detalhe = detalhe.PreencherValorNaLinha(143, 146, "0000");
                detalhe = detalhe.PreencherValorNaLinha(147, 147, " ");
                detalhe = detalhe.PreencherValorNaLinha(148, 149,
                    boleto.Especie.Sigla.Equals("DM") ? "01" : boleto.Especie.Codigo.ToString());
                detalhe = detalhe.PreencherValorNaLinha(150, 150, boleto.Aceite.Equals("A") ? "A" : "N");
                detalhe = detalhe.PreencherValorNaLinha(151, 156, boleto.DataDocumento.ToString("ddMMyy"));

                #region INSTRUÇÕES REMESSA

                if (boleto.InstrucoesDoBoleto.Count > 2)
                    throw new Exception(string.Format("<BoletoBr>{0}" +
                                                      "Não são aceitas mais que 2 instruções padronizadas para remessa de boletos no Banco do Brasil.",
                        Environment.NewLine));

                var primeiraInstrucao = boleto.InstrucoesDoBoleto.FirstOrDefault();
                var segundaInstrucao = boleto.InstrucoesDoBoleto.LastOrDefault();

                if (primeiraInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, primeiraInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(157, 158, "00");
                if (segundaInstrucao != null)
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, segundaInstrucao.ToString());
                else
                    detalhe = detalhe.PreencherValorNaLinha(159, 160, "00");

                #endregion

                #region JUROS DE MORA POR DIA DE ATRASO

                var jurosPorDia = string.Empty;

                if (boleto.JurosMora.ToString().Contains('.') && boleto.JurosMora.ToString().Contains(','))
                {
                    jurosPorDia = boleto.JurosMora.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.ToString().PadLeft(13, '0'));
                }
                if (boleto.JurosMora.ToString().Contains('.'))
                {
                    jurosPorDia = boleto.JurosMora.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.ToString().PadLeft(13, '0'));
                }
                if (boleto.JurosMora.ToString().Contains(','))
                {
                    jurosPorDia = boleto.JurosMora.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(161, 173, jurosPorDia.ToString().PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(161, 173, boleto.JurosMora.ToString().PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(174, 179, boleto.DataLimite.ToString("ddMMyy"));

                #region DESCONTO

                var descontoBoleto = string.Empty;

                if (boleto.ValorDesconto.ToString().Contains('.') && boleto.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.ValorDesconto.ToString().Contains('.'))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.ValorDesconto.ToString().Contains(','))
                {
                    descontoBoleto = boleto.ValorDesconto.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(180, 192, descontoBoleto.ToString().PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(180, 192, boleto.ValorDesconto.ToString().PadLeft(13, '0'));

                #endregion

                #region IOF

                var iofBoleto = string.Empty;

                if (boleto.Iof.ToString("F5").Contains('.') && boleto.Iof.ToString("F5").Contains(','))
                {
                    iofBoleto = boleto.Iof.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.Iof.ToString("F5").Contains('.'))
                {
                    iofBoleto = boleto.Iof.ToString("F5").Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.Iof.ToString("F5").Contains(','))
                {
                    iofBoleto = boleto.Iof.ToString("F5").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, iofBoleto.ToString().PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(193, 205, boleto.Iof.ToString("F5").Replace(",", "").PadLeft(13, '0'));

                #endregion

                #region ABATIMENTO

                var abatimentoBoleto = string.Empty;

                if (boleto.ValorAbatimento.ToString().Contains('.') && boleto.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(".", "").Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, abatimentoBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.ValorAbatimento.ToString().Contains('.'))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(".", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, abatimentoBoleto.ToString().PadLeft(13, '0'));
                }
                if (boleto.ValorAbatimento.ToString().Contains(','))
                {
                    abatimentoBoleto = boleto.ValorAbatimento.ToString().Replace(",", "");
                    detalhe = detalhe.PreencherValorNaLinha(193, 205, abatimentoBoleto.ToString().PadLeft(13, '0'));
                }

                detalhe = detalhe.PreencherValorNaLinha(193, 205, boleto.ValorAbatimento.ToString().Replace(",", "").PadLeft(13, '0'));

                #endregion

                detalhe = detalhe.PreencherValorNaLinha(219, 220,
                    boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").Length == 11
                        ? "01"
                        : "02");
                detalhe = detalhe.PreencherValorNaLinha(221, 234,
                    boleto.SacadoBoleto.CpfCnpj.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0'));
                detalhe = detalhe.PreencherValorNaLinha(235, 271, boleto.SacadoBoleto.Nome.ToUpper().PadRight(37, ' '));
                detalhe = detalhe.PreencherValorNaLinha(272, 274, string.Empty.PadRight(3, ' '));
                detalhe = detalhe.PreencherValorNaLinha(275, 314, enderecoSacado.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(315, 326,
                    boleto.SacadoBoleto.EnderecoSacado.Bairro.PadRight(12, ' '));

                var Cep = boleto.SacadoBoleto.EnderecoSacado.Cep;

                if (Cep.Contains(".") && Cep.Contains("-"))
                    Cep = Cep.Replace(".", "").Replace("-", "");
                if (Cep.Contains("."))
                    Cep = Cep.Replace(".", "");
                if (Cep.Contains("-"))
                    Cep = Cep.Replace("-", "");

                detalhe = detalhe.PreencherValorNaLinha(327, 334, Cep.PadLeft(8, ' ')); // Cep do Sacado
                detalhe = detalhe.PreencherValorNaLinha(335, 349,
                    boleto.SacadoBoleto.EnderecoSacado.Cidade.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(350, 351,
                    boleto.SacadoBoleto.EnderecoSacado.SiglaUf.PadRight(15, ' '));
                detalhe = detalhe.PreencherValorNaLinha(352, 391, string.Empty.PadRight(40, ' '));
                detalhe = detalhe.PreencherValorNaLinha(392, 393, boleto.QtdDias.ToString().PadLeft(2, '0'));
                detalhe = detalhe.PreencherValorNaLinha(394, 394, " ");
                detalhe = detalhe.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do DETALHE do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }

        public string EscreverTrailer(int numeroRegistro)
        {
            if (numeroRegistro == 0)
                throw new Exception("Sequencial do registro não foi informado na geração do TRAILER.");

            var trailer = new string(' ', 400);
            try
            {
                trailer = trailer.PreencherValorNaLinha(1, 1, "9");
                trailer = trailer.PreencherValorNaLinha(2, 394, string.Empty.PadRight(393, ' '));
                // Contagem total de linhas do arquivo no formato '000000' - 6 dígitos
                trailer = trailer.PreencherValorNaLinha(395, 400, numeroRegistro.ToString().PadLeft(6, '0'));

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("<BoletoBr>{0}Falha na geração do TRAILER do arquivo de REMESSA.",
                    Environment.NewLine), e);
            }
        }
    }
}