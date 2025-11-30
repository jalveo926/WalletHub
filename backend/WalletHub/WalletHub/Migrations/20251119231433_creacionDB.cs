using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WalletHub.Migrations
{
    /// <inheritdoc />
    public partial class creacionDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    nombreUsu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    correoUsu = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    pwHashUsu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    idCategoria = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    nombreCateg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tipoCateg = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    idUsuario = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.idCategoria);
                    table.ForeignKey(
                        name: "FK_Categoria_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    idReporte = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    rutaArchivoRepo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    tipoArchivoRepo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fechaCreacionRepo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    inicioPeriodo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    finalPeriodo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idUsuario = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.idReporte);
                    table.ForeignKey(
                        name: "FK_Reporte_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion",
                columns: table => new
                {
                    idTransaccion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    fechaTransac = table.Column<DateTime>(type: "datetime2", nullable: false),
                    montoTransac = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    descripcionTransac = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    idUsuario = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    idCategoria = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.idTransaccion);
                    table.ForeignKey(
                        name: "FK_Transaccion_Categoria_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "Categoria",
                        principalColumn: "idCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaccion_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "idCategoria", "idUsuario", "nombreCateg", "tipoCateg" },
                values: new object[,]
                {
                    { "CA001", null, "Hogar", "Gasto" },
                    { "CA002", null, "Alimentación", "Gasto" },
                    { "CA003", null, "Transporte", "Gasto" },
                    { "CA004", null, "Salud", "Gasto" },
                    { "CA005", null, "Educación", "Gasto" },
                    { "CA006", null, "Entretenimiento", "Gasto" },
                    { "CA007", null, "Salario", "Ingreso" },
                    { "CA008", null, "Emprendimiento Personal", "Ingreso" },
                    { "CA009", null, "Inversiones", "Ingreso" },
                    { "CA010", null, "Regalos", "Ingreso" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_idUsuario",
                table: "Categoria",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_idUsuario",
                table: "Reporte",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_idCategoria",
                table: "Transaccion",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_idUsuario",
                table: "Transaccion",
                column: "idUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "Transaccion");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
