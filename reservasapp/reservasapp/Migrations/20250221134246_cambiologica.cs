using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservasapp.Migrations
{
    /// <inheritdoc />
    public partial class cambiologica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reserva_FechaHora",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "HoraApertura",
                table: "HorarioTrabajo");

            migrationBuilder.DropColumn(
                name: "HoraCierre",
                table: "HorarioTrabajo");

            migrationBuilder.DropColumn(
                name: "Intervalo",
                table: "HorarioTrabajo");

            migrationBuilder.RenameColumn(
                name: "FechaHora",
                table: "Reserva",
                newName: "Fecha");

            migrationBuilder.AlterColumn<string>(
                name: "Cliente",
                table: "Reserva",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Turno",
                table: "Reserva",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HoraFin",
                table: "HorarioTrabajo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HoraInicio",
                table: "HorarioTrabajo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_Cliente_Fecha",
                table: "Reserva",
                columns: new[] { "Cliente", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_Fecha_Turno",
                table: "Reserva",
                columns: new[] { "Fecha", "Turno" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reserva_Cliente_Fecha",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_Fecha_Turno",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "Turno",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "HorarioTrabajo");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "HorarioTrabajo");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Reserva",
                newName: "FechaHora");

            migrationBuilder.AlterColumn<string>(
                name: "Cliente",
                table: "Reserva",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraApertura",
                table: "HorarioTrabajo",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraCierre",
                table: "HorarioTrabajo",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Intervalo",
                table: "HorarioTrabajo",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_FechaHora",
                table: "Reserva",
                column: "FechaHora",
                unique: true);
        }
    }
}
