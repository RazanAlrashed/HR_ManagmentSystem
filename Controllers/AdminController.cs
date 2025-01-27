using HR_ManagmentSystem.Models.BaseModel;
using HR_ManagmentSystem.Models.Domain;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml; 
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HR_ManagmentSystem.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public AdminController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Employee()
        {
            var getdataDepartment = _databaseContext.Departments.ToList();
            ViewBag.getdataDepartment = getdataDepartment;
            var getdataDesignation = _databaseContext.Designations.ToList();
            ViewBag.getdataDesignation = getdataDesignation;
            return View();
        }

        public IActionResult AddEmployee(Employee employee)
        {
            _databaseContext.Add(employee);
            _databaseContext.SaveChanges();
            return RedirectToAction("Employee");

        }
        public IActionResult Designation()
        {
            var getdata = _databaseContext.Designations.ToList();
            return View(getdata);
        }
        public IActionResult AddDesignation(Designation designation) 
        {
            _databaseContext.Add(designation);
            _databaseContext.SaveChanges();
            return RedirectToAction("Designation");
        }

        public IActionResult DeletDesignation(int id)
        {
            var getdata = _databaseContext.Designations.SingleOrDefault(d=> d.Id==id);
            if (getdata != null)
            {
                _databaseContext.Remove(getdata);
                _databaseContext.SaveChanges();
            }
            return RedirectToAction("Designation");

        }

        public IActionResult EditDesignation(int id)
        {
            var editdesignation = _databaseContext.Designations.SingleOrDefault(x=> x.Id==id);
            return View(editdesignation);
        }

        public IActionResult UpdateDesignation(Designation designation)
        {
            _databaseContext.Update(designation);
            _databaseContext.SaveChanges();
            return RedirectToAction("Designation");
        }
        public IActionResult Department()
        {
            var getdata = _databaseContext.Departments.ToList();
            return View(getdata);
        }
        public IActionResult AddDepartment(Department department)
        {
            _databaseContext.Add(department);
            _databaseContext.SaveChanges();
            return RedirectToAction("Department");
        }
        public IActionResult DeletDepartment(int id)
        {
            var getdata = _databaseContext.Departments.SingleOrDefault(e=>e.Id==id);
            if (getdata != null)
            {
                _databaseContext.Remove(getdata);
                _databaseContext.SaveChanges();
            }
            return RedirectToAction("Department");
        }

        public IActionResult EditDepartment(int id)
        {
            var editdepartment = _databaseContext.Departments.SingleOrDefault(d=> d.Id==id);
            return View(editdepartment);
        }

        public IActionResult UpdateDepartment(Department department)
        {
            
            _databaseContext.Update(department);
            _databaseContext.SaveChanges();
            return RedirectToAction("Department");
        }

        public IActionResult EmployeeList()
        {
            var getEmployeeInfo = _databaseContext.Employees.Join(
                _databaseContext.Departments,
                employee => employee.Department,
                department => department.Id,
                (employee, department) => new { employee, department }
                )
                .Join(
                _databaseContext.Designations,
                combined => combined.employee.DesignationName, // Match Employee.DesignationName with Designation.Designations
                designation => designation.Id,
                (combined, designation) => new
                {
                    EmployeeId = combined.employee.Id,
                    EmployeeNumber = combined.employee.EmployeeNumber,
                    EmployeeName = combined.employee.EmployeeName,
                    Email = combined.employee.Email,
                    ContactNumber = combined.employee.ContactNumber,
                    DepartmentName = combined.department.DepartmentName,
                    DesignationName = designation.Designations,
                    Salary = combined.employee.Salary,
                    BankName = combined.employee.BankName,
                    BankAccountNumber = combined.employee.BankAccountNumber,
                    Nationality = combined.employee.Nationality,
                    Address = combined.employee.Address
                }).ToList();


            
            ViewBag.getEmployeeInfo = getEmployeeInfo;
            return View();
        }

        public IActionResult DeletEmployee(int id)
        {
            var getemployee = _databaseContext.Employees.SingleOrDefault(emp => emp.Id == id);
            if (getemployee != null)
            {
                _databaseContext.Remove(getemployee);
                _databaseContext.SaveChanges();
            }
            return RedirectToAction("EmployeeList");

        }

        public IActionResult EditEmployee(int id)
        {
            var getdataDepartment = _databaseContext.Departments.ToList();
            ViewBag.getdataDepartment = getdataDepartment;
            var getdataDesignation = _databaseContext.Designations.ToList();
            ViewBag.getdataDesignation = getdataDesignation;
            var getemployeedata= _databaseContext.Employees.SingleOrDefault(x=> x.Id == id);
            return View(getemployeedata);
        }
            
        public IActionResult UpdateEmployee(Employee employee)
        {
            _databaseContext.Update(employee);
            _databaseContext.SaveChanges();
            return RedirectToAction("EmployeeList");

        }

        public IActionResult ExportEmployeeListToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var EmployeeListData = _databaseContext.Employees
            .Join(
                    _databaseContext.Departments,
                    employee => employee.Department,
                    department => department.Id,
                    (employee, department) => new { employee, department }
            )
            .Join(
                    _databaseContext.Designations,
                    combined => combined.employee.DesignationName,
                    designation => designation.Id,
                    (combined, designation) => new
                    {
                        EmployeeName = combined.employee.EmployeeName,
                        EmployeeNumber = combined.employee.EmployeeNumber,
                        DepartmentName = combined.department.DepartmentName,
                        DesignationName = designation.Designations,
                        Salary = combined.employee.Salary,
                        BankName = combined.employee.BankName,
                        BankAccountNumber = combined.employee.BankAccountNumber,
                        Nationality = combined.employee.Nationality,
                        Address = combined.employee.Address
                    }
                )
                .ToList();

            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("SalaryReport");

            worksheet.Cells[1, 1].Value = "Employee Number";
            worksheet.Cells[1, 2].Value = "Employee Name";
            worksheet.Cells[1, 3].Value = "Department";
            worksheet.Cells[1, 4].Value = "Designation";
            worksheet.Cells[1, 5].Value = "Salary";
            worksheet.Cells[1, 6].Value = "Bank Name";
            worksheet.Cells[1, 7].Value = "Bank Account Number";
            worksheet.Cells[1, 8].Value = "Nationality";
            worksheet.Cells[1, 9].Value = "Address";


            int row = 2;
            foreach (var item in EmployeeListData)
            {
                worksheet.Cells[row, 1].Value = item.EmployeeNumber;
                worksheet.Cells[row, 2].Value = item.EmployeeName;
                worksheet.Cells[row, 3].Value = item.DepartmentName;
                worksheet.Cells[row, 4].Value = item.DesignationName;
                worksheet.Cells[row, 5].Value = item.Salary;
                worksheet.Cells[row, 6].Value = item.BankName;
                worksheet.Cells[row, 7].Value = item.BankAccountNumber;
                worksheet.Cells[row, 8].Value = item.Nationality;
                worksheet.Cells[row, 9].Value = item.Address;

                row++;
            }

            var fileContents = package.GetAsByteArray();
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalaryReport.xlsx");
        }


        public IActionResult LeaveApplications()
        {
            var leaveApplications = _databaseContext.LeaveApplays
                .Join(
                _databaseContext.Employees,
                leave => leave.EmployeeId,
                employee => employee.Id,
                (leave, employee) => new
                {
                    LeaveId = leave.Id,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    NumberOfDay = leave.NumberOfDay,
                    Reason = leave.Reason,
                    Status = leave.Status,
                    EmployeeName = employee.EmployeeName,
                    EmployeeNumber = employee.EmployeeNumber
                }).ToList();
            ViewBag.LeaveApplications = leaveApplications;
            return View();
        }

        public IActionResult ApproveLeave(int id)
        {
            var leaveApplication = _databaseContext.LeaveApplays.FirstOrDefault(l => l.Id == id);
            if (leaveApplication != null)
            {
                leaveApplication.Status = "Approved";
                _databaseContext.SaveChanges();
            }
            return RedirectToAction("LeaveApplications");
        }

        public IActionResult RejectLeave(int id)
        {
            var leaveApplication = _databaseContext.LeaveApplays.FirstOrDefault(l => l.Id == id);
            if (leaveApplication != null)
            {
                leaveApplication.Status = "Rejected";
                _databaseContext.SaveChanges();
            }
            return RedirectToAction("LeaveApplications");
        }

        public IActionResult SalaryReport()
        {

            var salaryReportData = _databaseContext.Employees
        .Join(
            _databaseContext.Departments,
            employee => employee.Department,
            department => department.Id,
            (employee, department) => new { employee, department }
        )
        .Join(
            _databaseContext.Designations,
            combined => combined.employee.DesignationName,
            designation => designation.Id,
            (combined, designation) => new
            {
                EmployeeId = combined.employee.Id,
                EmployeeName = combined.employee.EmployeeName,
                EmployeeNumber = combined.employee.EmployeeNumber,
                DepartmentName = combined.department.DepartmentName,
                DesignationName = designation.Designations,
                Salary = combined.employee.Salary,
                BankName = combined.employee.BankName,
                BankAccountNumber = combined.employee.BankAccountNumber
            }
        )
        .ToList();

            ViewBag.SalaryReportData = salaryReportData;
            return View();
        }


        public IActionResult ExportSalaryReportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var salaryReportData = _databaseContext.Employees
            .Join(
                    _databaseContext.Departments,
                    employee => employee.Department,
                    department => department.Id,
                    (employee, department) => new { employee, department }
            )
            .Join(
                    _databaseContext.Designations,
                    combined => combined.employee.DesignationName,
                    designation => designation.Id,
                    (combined, designation) => new
                    {
                        EmployeeName = combined.employee.EmployeeName,
                        EmployeeNumber = combined.employee.EmployeeNumber,
                        DepartmentName = combined.department.DepartmentName,
                        DesignationName = designation.Designations,
                        Salary = combined.employee.Salary,
                        BankName = combined.employee.BankName,
                        BankAccountNumber = combined.employee.BankAccountNumber
                    }
                )
                .ToList();

            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("SalaryReport");

            worksheet.Cells[1, 1].Value = "Employee Number";
            worksheet.Cells[1, 2].Value = "Employee Name";
            worksheet.Cells[1, 3].Value = "Department";
            worksheet.Cells[1, 4].Value = "Designation";
            worksheet.Cells[1, 5].Value = "Salary";
            worksheet.Cells[1, 6].Value = "Bank Name";
            worksheet.Cells[1, 7].Value = "Bank Account Number";

            int row = 2;
            foreach (var item in salaryReportData)
            {
                worksheet.Cells[row, 1].Value = item.EmployeeNumber;
                worksheet.Cells[row, 2].Value = item.EmployeeName;
                worksheet.Cells[row, 3].Value = item.DepartmentName;
                worksheet.Cells[row, 4].Value = item.DesignationName;
                worksheet.Cells[row, 5].Value = item.Salary;
                worksheet.Cells[row, 6].Value = item.BankName;
                worksheet.Cells[row, 7].Value = item.BankAccountNumber;
                row++;
            }

            var fileContents = package.GetAsByteArray();
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalaryReport.xlsx");
        }




    }
}
