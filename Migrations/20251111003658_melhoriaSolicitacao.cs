using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ActiveControlApi.Migrations
{
    /// <inheritdoc />
    public partial class melhoriaSolicitacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Solicitacao",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFechamento",
                table: "Solicitacao",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrazo",
                table: "Solicitacao",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Prioridade",
                table: "Solicitacao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Solicitacao",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioResponsavelId",
                table: "Solicitacao",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CustoReal",
                table: "Manutencao",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAgendada",
                table: "Manutencao",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Manutencao",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrazo",
                table: "Manutencao",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Manutencao",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Prioridade",
                table: "Manutencao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SolucaoAplicada",
                table: "Manutencao",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusManutencao",
                table: "Manutencao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioResponsavelId",
                table: "Manutencao",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Severidade",
                table: "Incidente",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Incidente",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "CausaRaiz",
                table: "Incidente",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAbertura",
                table: "Incidente",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicioResolucao",
                table: "Incidente",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrazo",
                table: "Incidente",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataResolucao",
                table: "Incidente",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Prioridade",
                table: "Incidente",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SolucaoAplicada",
                table: "Incidente",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusIncidente",
                table: "Incidente",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioResponsavelId",
                table: "Incidente",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_UsuarioResponsavelId",
                table: "Solicitacao",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencao_UsuarioResponsavelId",
                table: "Manutencao",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidente_UsuarioResponsavelId",
                table: "Incidente",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioSolicitacao_SolicitacaoId",
                table: "ComentarioSolicitacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioSolicitacao_UsuarioId",
                table: "ComentarioSolicitacao",
                column: "UsuarioId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Incidente_Usuario_UsuarioResponsavelId",
                table: "Incidente",
                column: "UsuarioResponsavelId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manutencao_Usuario_UsuarioResponsavelId",
                table: "Manutencao",
                column: "UsuarioResponsavelId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_Usuario_UsuarioResponsavelId",
                table: "Solicitacao",
                column: "UsuarioResponsavelId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidente_Usuario_UsuarioResponsavelId",
                table: "Incidente");

            migrationBuilder.DropForeignKey(
                name: "FK_Manutencao_Usuario_UsuarioResponsavelId",
                table: "Manutencao");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_Usuario_UsuarioResponsavelId",
                table: "Solicitacao");

            migrationBuilder.DropTable(
                name: "ComentarioSolicitacao");

            migrationBuilder.DropTable(
                name: "HistoricoMovimentacao");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacao_UsuarioResponsavelId",
                table: "Solicitacao");

            migrationBuilder.DropIndex(
                name: "IX_Manutencao_UsuarioResponsavelId",
                table: "Manutencao");

            migrationBuilder.DropIndex(
                name: "IX_Incidente_UsuarioResponsavelId",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "DataPrazo",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "UsuarioResponsavelId",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "CustoReal",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "DataAgendada",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "DataPrazo",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "SolucaoAplicada",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "StatusManutencao",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "UsuarioResponsavelId",
                table: "Manutencao");

            migrationBuilder.DropColumn(
                name: "CausaRaiz",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "DataAbertura",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "DataInicioResolucao",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "DataPrazo",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "DataResolucao",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "SolucaoAplicada",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "StatusIncidente",
                table: "Incidente");

            migrationBuilder.DropColumn(
                name: "UsuarioResponsavelId",
                table: "Incidente");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Solicitacao",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFechamento",
                table: "Solicitacao",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Severidade",
                table: "Incidente",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Incidente",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);
        }
    }
}
