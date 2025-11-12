using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ActiveControlApi.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoActiveControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    CBO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaAtivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeCategoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaAtivo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RazaoSocial = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NomeFantasia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TelContato = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    EnderecoEmpresa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CidadeEmpresa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UfEmpresa = table.Column<string>(type: "char(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeloAtivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Fabricante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Especificacoes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloAtivo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeCompleto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SenhaHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    SenhaSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    TokenDataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    EmpresaId = table.Column<int>(type: "integer", nullable: false),
                    EnderecoSetor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CidadeSetor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UfSetor = table.Column<string>(type: "char(2)", nullable: false),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    LocalizacaoInterna = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamento_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ativo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoNome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumPatrimonio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumSerie = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    ValorAquisicao = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "date", nullable: false),
                    StatusAtivo = table.Column<int>(type: "integer", nullable: true),
                    ModeloAtivoId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaAtivoId = table.Column<int>(type: "integer", nullable: false),
                    VidaUtilEstimadaAnos = table.Column<int>(type: "integer", nullable: false),
                    TaxaDepreciacaoAnual = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ativo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ativo_CategoriaAtivo_CategoriaAtivoId",
                        column: x => x.CategoriaAtivoId,
                        principalTable: "CategoriaAtivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ativo_ModeloAtivo_ModeloAtivoId",
                        column: x => x.ModeloAtivoId,
                        principalTable: "ModeloAtivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCargo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    CargoId = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCargo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioCargo_Cargo_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCargo_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioDepartamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartamentoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioDepartamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioDepartamento_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioDepartamento_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtivoDepartamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    DepartamentoId = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtivoDepartamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtivoDepartamento_Ativo_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "Ativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtivoDepartamento_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtivoUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtivoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtivoUsuario_Ativo_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "Ativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtivoUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UsuarioSolicitanteId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioResponsavelId = table.Column<int>(type: "integer", nullable: true),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    TipoSolicitacao = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    StatusSolicitacao = table.Column<int>(type: "integer", nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPrazo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFechamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Ativo_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "Ativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Usuario_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Solicitacao_Usuario_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComentarioSolicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Comentario = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DataComentario = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Interno = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentarioSolicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComentarioSolicitacao_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentarioSolicitacao_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devolucao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    MotivoDevolucao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devolucao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devolucao_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoMovimentacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoMovimentacao = table.Column<int>(type: "integer", nullable: false),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    DepartamentoId = table.Column<int>(type: "integer", nullable: true),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DadosAnteriores = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DadosNovos = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    UsuarioResponsavelId = table.Column<int>(type: "integer", nullable: false),
                    DataMovimentacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoMovimentacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoMovimentacao_Ativo_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "Ativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoMovimentacao_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoMovimentacao_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoMovimentacao_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoMovimentacao_Usuario_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    Severidade = table.Column<int>(type: "integer", nullable: false),
                    StatusIncidente = table.Column<int>(type: "integer", nullable: false),
                    UsuarioResponsavelId = table.Column<int>(type: "integer", nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataInicioResolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataResolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataPrazo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SolucaoAplicada = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CausaRaiz = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidente_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidente_Usuario_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Manutencao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    TipoManutencao = table.Column<int>(type: "integer", nullable: false),
                    StatusManutencao = table.Column<int>(type: "integer", nullable: false),
                    UsuarioResponsavelId = table.Column<int>(type: "integer", nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAgendada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFechamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataPrazo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustoEstimado = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CustoReal = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SolucaoAplicada = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manutencao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manutencao_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manutencao_Usuario_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ativo_CategoriaAtivoId",
                table: "Ativo",
                column: "CategoriaAtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ativo_ModeloAtivoId",
                table: "Ativo",
                column: "ModeloAtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtivoDepartamento_AtivoId",
                table: "AtivoDepartamento",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtivoDepartamento_DepartamentoId",
                table: "AtivoDepartamento",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtivoUsuario_AtivoId",
                table: "AtivoUsuario",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtivoUsuario_UsuarioId",
                table: "AtivoUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioSolicitacao_SolicitacaoId",
                table: "ComentarioSolicitacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioSolicitacao_UsuarioId",
                table: "ComentarioSolicitacao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamento_EmpresaId",
                table: "Departamento",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucao_SolicitacaoId",
                table: "Devolucao",
                column: "SolicitacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoMovimentacao_AtivoId",
                table: "HistoricoMovimentacao",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoMovimentacao_DepartamentoId",
                table: "HistoricoMovimentacao",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoMovimentacao_SolicitacaoId",
                table: "HistoricoMovimentacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoMovimentacao_UsuarioId",
                table: "HistoricoMovimentacao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoMovimentacao_UsuarioResponsavelId",
                table: "HistoricoMovimentacao",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidente_SolicitacaoId",
                table: "Incidente",
                column: "SolicitacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidente_UsuarioResponsavelId",
                table: "Incidente",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencao_SolicitacaoId",
                table: "Manutencao",
                column: "SolicitacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manutencao_UsuarioResponsavelId",
                table: "Manutencao",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_AtivoId",
                table: "Solicitacao",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_UsuarioResponsavelId",
                table: "Solicitacao",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_UsuarioSolicitanteId",
                table: "Solicitacao",
                column: "UsuarioSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCargo_CargoId",
                table: "UsuarioCargo",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCargo_UsuarioId",
                table: "UsuarioCargo",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioDepartamento_DepartamentoId",
                table: "UsuarioDepartamento",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioDepartamento_UsuarioId",
                table: "UsuarioDepartamento",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtivoDepartamento");

            migrationBuilder.DropTable(
                name: "AtivoUsuario");

            migrationBuilder.DropTable(
                name: "ComentarioSolicitacao");

            migrationBuilder.DropTable(
                name: "Devolucao");

            migrationBuilder.DropTable(
                name: "HistoricoMovimentacao");

            migrationBuilder.DropTable(
                name: "Incidente");

            migrationBuilder.DropTable(
                name: "Manutencao");

            migrationBuilder.DropTable(
                name: "UsuarioCargo");

            migrationBuilder.DropTable(
                name: "UsuarioDepartamento");

            migrationBuilder.DropTable(
                name: "Solicitacao");

            migrationBuilder.DropTable(
                name: "Cargo");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "Ativo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "CategoriaAtivo");

            migrationBuilder.DropTable(
                name: "ModeloAtivo");
        }
    }
}
