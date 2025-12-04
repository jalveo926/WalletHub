using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WalletHub.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_Usuario_idUsuario",
                table: "Categoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Reporte_Usuario_idUsuario",
                table: "Reporte");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaccion_Categoria_idCategoria",
                table: "Transaccion");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaccion_Usuario_idUsuario",
                table: "Transaccion");

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "idUsuario", "correoUsu", "nombreUsu", "pwHashUsu" },
                values: new object[,]
                {
                    { "US001", "jesus.alveo@utp.ac.pa", "Jesus", "AQAAAAIAAYagAAAAELTGLo7+TfUQMvP8jqKL5+BbQSorW9tghAnPuU2xuvpqifTarMSVYOKMm1V0Cq/XHw==" },
                    { "US002", "erick.hou@utp.ac.pa", "Erick", "AQAAAAIAAYagAAAAEB45Z6Gu4V/N2CkXvobsT6xIH8lAkdIE7qsAlZJRFHi/tmy5u7G6uWhflAFuQvhh/w==" },
                    { "US003", "roniel.quintero@utp.ac.pa", "Roniel", "AQAAAAIAAYagAAAAEIDl+SkyIySpcBx88SBrCDMNZVlj/4Wfkqxcbbv1BNTNG10+6aOVCX2/JFHX+FrSsg==" },
                    { "US004", "jessica.zheng@utp.ac.pa", "Jessica", "AQAAAAIAAYagAAAAEF8fIe8K49ZHrRycUBl5W8tpc2d+vpxKQI00CG2JuxHSGLvo/hf/ZJCUiH9/ZeZYdQ==" }
                });

            migrationBuilder.InsertData(
                table: "Transaccion",
                columns: new[] { "idTransaccion", "descripcionTransac", "fechaTransac", "idCategoria", "idUsuario", "montoTransac" },
                values: new object[,]
                {
                    { "TR001", "Compra de supermercado", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "CA002", "US001", 150.75m },
                    { "TR002", "Pago de salario mensual", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "CA007", "US001", 2000.00m },
                    { "TR003", "Taxi al trabajo", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "CA003", "US002", 50.00m },
                    { "TR004", "Cena con amigos", new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "CA006", "US003", 300.00m },
                    { "TR005", "Pago de salario mensual", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CA007", "US004", 1200.00m }
                });

            migrationBuilder.AddForeignKey(
                name: "Categoria_idUsuario_FK",
                table: "Categoria",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Reporte_idUsuario_FK",
                table: "Reporte",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Transaccion_idCategoria_FK",
                table: "Transaccion",
                column: "idCategoria",
                principalTable: "Categoria",
                principalColumn: "idCategoria",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Transaccion_idUsuario_FK",
                table: "Transaccion",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Categoria_idUsuario_FK",
                table: "Categoria");

            migrationBuilder.DropForeignKey(
                name: "Reporte_idUsuario_FK",
                table: "Reporte");

            migrationBuilder.DropForeignKey(
                name: "Transaccion_idCategoria_FK",
                table: "Transaccion");

            migrationBuilder.DropForeignKey(
                name: "Transaccion_idUsuario_FK",
                table: "Transaccion");

            migrationBuilder.DeleteData(
                table: "Transaccion",
                keyColumn: "idTransaccion",
                keyValue: "TR001");

            migrationBuilder.DeleteData(
                table: "Transaccion",
                keyColumn: "idTransaccion",
                keyValue: "TR002");

            migrationBuilder.DeleteData(
                table: "Transaccion",
                keyColumn: "idTransaccion",
                keyValue: "TR003");

            migrationBuilder.DeleteData(
                table: "Transaccion",
                keyColumn: "idTransaccion",
                keyValue: "TR004");

            migrationBuilder.DeleteData(
                table: "Transaccion",
                keyColumn: "idTransaccion",
                keyValue: "TR005");

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "idUsuario",
                keyValue: "US001");

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "idUsuario",
                keyValue: "US002");

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "idUsuario",
                keyValue: "US003");

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "idUsuario",
                keyValue: "US004");

            migrationBuilder.AddForeignKey(
                name: "FK_Categoria_Usuario_idUsuario",
                table: "Categoria",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reporte_Usuario_idUsuario",
                table: "Reporte",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaccion_Categoria_idCategoria",
                table: "Transaccion",
                column: "idCategoria",
                principalTable: "Categoria",
                principalColumn: "idCategoria",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaccion_Usuario_idUsuario",
                table: "Transaccion",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
