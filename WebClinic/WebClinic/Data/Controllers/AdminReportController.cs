using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebClinic.Data.Context;
using WebClinic.Data.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebClinic.Controllers
{
    [Authorize(Roles = "Админ")]
    [Route("[controller]")]
    [ApiController]
    public class AdminReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("generateReport")]
        public async Task<IActionResult> GenerateReport()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            var reportData = await _context.Patients
                .Include(p => p.MedicalCards)
                    .ThenInclude(mc => mc.Appointments)
                    .ThenInclude(a => a.Schedule)
                        .ThenInclude(s => s.Service)
                            .ThenInclude(s => s.Doctor)
                .Select(p => new
                {
                    p.User.UserName,
                    p.DateOfBirth,
                    p.Height,
                    p.Weight,
                    Appointments = p.Appointments
                        .Where(a => a.Schedule != null)
                        .Select(a => new
                        {
                            AppointmentDate = a.Schedule.Date,
                            StartTime = a.Schedule.StartTime,
                            DoctorName = a.Schedule.Service.Doctor.User.UserName,
                            ServiceName = a.Schedule.Service.ServiceName,
                            ServiceCabinet = a.Schedule.Service.Cabinet,
                            ServiceCost = a.Schedule.Service.Cost,
                            ServiceDescription = a.Schedule.Service.Description,
                            Diagnosis = a.AppointmentResults.FirstOrDefault().Diagnosis ?? "Нет диагноза",
                            Prescription = a.AppointmentResults.FirstOrDefault().Prescription ?? "Нет рецепта"
                        })
                })
                .ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчет");

            // Заполнение заголовков
            worksheet.Cells[1, 1].Value = "№";
            worksheet.Cells[1, 2].Value = "Email пациента";
            worksheet.Cells[1, 3].Value = "Дата рождения";
            worksheet.Cells[1, 4].Value = "Рост (см)";
            worksheet.Cells[1, 5].Value = "Вес (кг)";
            worksheet.Cells[1, 6].Value = "Дата записи";
            worksheet.Cells[1, 7].Value = "Начало приема";
            worksheet.Cells[1, 8].Value = "Email врача";
            worksheet.Cells[1, 9].Value = "Услуга";
            worksheet.Cells[1, 10].Value = "Кабинет";
            worksheet.Cells[1, 11].Value = "Стоимость услуги";
            worksheet.Cells[1, 12].Value = "Описание услуги";
            worksheet.Cells[1, 13].Value = "Диагноз";
            worksheet.Cells[1, 14].Value = "Рецепт";

            // Установка ширины столбцов
            worksheet.Column(1).Width = 5;   // Номер строки
            worksheet.Column(2).Width = 20;  // Email пациента
            worksheet.Column(3).Width = 15;  // Дата рождения
            worksheet.Column(4).Width = 10;  // Рост
            worksheet.Column(5).Width = 10;  // Вес
            worksheet.Column(6).Width = 15;  // Дата записи
            worksheet.Column(7).Width = 12;  // Начало приема
            worksheet.Column(8).Width = 20;  // Email врача
            worksheet.Column(9).Width = 15;  // Услуга
            worksheet.Column(10).Width = 15; // Кабинет
            worksheet.Column(11).Width = 15; // Стоимость услуги
            worksheet.Column(12).Width = 30; // Описание услуги
            worksheet.Column(13).Width = 50; // Диагноз
            worksheet.Column(14).Width = 50; // Рецепт

            // Применение рамки к заголовкам
            using (var range = worksheet.Cells[1, 1, 1, 14])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }

            // Выравнивание всех ячеек по центру
            worksheet.Cells[2, 1, reportData.Count + 1, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 1, reportData.Count + 1, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            int row = 2; // Стартовая строка для данных

            foreach (var patient in reportData)
            {
                foreach (var appointment in patient.Appointments)
                {
                    worksheet.Cells[row, 1].Value = row - 1; // Номер строки
                    worksheet.Cells[row, 2].Value = patient.UserName;
                    worksheet.Cells[row, 3].Value = patient.DateOfBirth.ToShortDateString();
                    worksheet.Cells[row, 4].Value = patient.Height.ToString();
                    worksheet.Cells[row, 5].Value = patient.Weight.ToString();
                    worksheet.Cells[row, 6].Value = appointment.AppointmentDate.ToShortDateString();
                    worksheet.Cells[row, 7].Value = appointment.StartTime.ToString();
                    worksheet.Cells[row, 8].Value = appointment.DoctorName;
                    worksheet.Cells[row, 9].Value = appointment.ServiceName;
                    worksheet.Cells[row, 10].Value = appointment.ServiceCabinet;
                    worksheet.Cells[row, 11].Value = appointment.ServiceCost.ToString("F2");
                    worksheet.Cells[row, 12].Value = appointment.ServiceDescription;
                    worksheet.Cells[row, 13].Value = appointment.Diagnosis;
                    worksheet.Cells[row, 14].Value = appointment.Prescription;
                    row++;
                }
            }

            var fileBytes = package.GetAsByteArray();
            var fileName = "СтатистическийОтчет_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
