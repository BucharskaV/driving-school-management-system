using System.Text.RegularExpressions;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class Instructor : User
{
    public virtual ICollection<Certification> Certifications { get; set; } = [];
    public virtual ICollection<InstructorSpecialization> Specializations{ get; set; } = [];
    
    private string _instructorCode;
    public string InstructorCode
    {
        get => _instructorCode;
        init
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Instructor's code cannot be empty");
            }
            Regex codeRegex = new Regex("^[A-Z]{2}\\d{5}$");
            if (!codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentException("Invalid instructor's code. The format is: 2 uppercase letters followed by 5 digits");
            }
            _instructorCode = value.Trim();
        }
    }
    
    private decimal _baseSalary;
    public decimal BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Base salary cannot be less than 0");
            }
            _baseSalary = value;
        }
    }
    
    private decimal? _bonus;
    public decimal? Bonus
    {
        get => _bonus;
        set
        {
            if (value != null && value < 0)
            {
                throw new ArgumentException("Bonus cannot be less than 0");
            }
            _bonus = value;
        }
    }
    
    public decimal TotalSalary => BaseSalary + (Bonus ?? 0);
    
    private string? _drivingLicenseNumber;
    public string? DrivingLicenseNumber
    {
        get
        {
            EnsureType(InstructorType.PracticalInstructor);
            return _drivingLicenseNumber;
        }
        set
        {
            EnsureType(InstructorType.PracticalInstructor);
            Regex codeRegex = new Regex("^[A-Z]{1,2}[0-9A-Z]{6,14}$");
            if(value != null && !codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentNullException("Invalid driving license number.");
            }
            _drivingLicenseNumber = value?.Trim();
        }
    }
    
    private string? _medicalCertificateNumber;
    public string? MedicalCertificateNumber
    {
        get
        {
            EnsureType(InstructorType.PracticalInstructor);
            return _medicalCertificateNumber;
        }
        set
        {
            EnsureType(InstructorType.PracticalInstructor);
            Regex codeRegex = new Regex("^[0-9]{6,12}$");
            if(value != null && !codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentException("Invalid medical certificate number.");
            }
            _medicalCertificateNumber = value?.Trim();
        }
    }
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = [];
    public virtual ICollection<LessonInstructor> LessonInstructors { get; set; } = [];
    
    private Instructor() : base() { }

    public Instructor(List<InstructorType> types, string firstName, string lastName, Role role, string pesel, string phoneNumber, string? email, string instructorCode, decimal baseSalary, decimal? bonus, string? drivingLicenseNumber = null, string? medicalCertificateNumber = null) : base(firstName, lastName, role, pesel, phoneNumber, email)
    {
        if (types == null || types.Count == 0)
            throw new ArgumentException("Instructor must have at least one specialization");

        InstructorCode = instructorCode;
        BaseSalary = baseSalary;
        Bonus = bonus;

        foreach (var type in types)
        {
            Specializations.Add(new InstructorSpecialization(this, type));
        }

        if (types.Contains(InstructorType.PracticalInstructor))
        {
            if (string.IsNullOrWhiteSpace(drivingLicenseNumber))
                throw new ArgumentException("Driving license is required for Practical Instructor");
            if (string.IsNullOrWhiteSpace(medicalCertificateNumber))
                throw new ArgumentException("Medical certificate is required for Practical Instructor");

            DrivingLicenseNumber = drivingLicenseNumber;
            MedicalCertificateNumber = medicalCertificateNumber;
        }
    }
    
    public bool CanTeach(InstructorType type)
    {
        return Specializations.Any(s => s.Type == type);
    }

    private bool HasType(InstructorType type)
    {
        return Specializations.Any(x => x.Type == type);
    }

    private void EnsureType(InstructorType type)
    {
        if (!HasType(type))
            throw new InvalidOperationException($"Instructor is not {type}");
    }
    
    public void AddPracticalSpecialization(string drivingLicenseNumber, string medicalCertificateNumber)
    {
        if (HasType(InstructorType.PracticalInstructor))
            throw new InvalidOperationException("Instructor already has Practical specialization");
        if (string.IsNullOrEmpty(drivingLicenseNumber))
            throw new ArgumentException("Driving license is required for Practical Instructor");
        if (string.IsNullOrEmpty(medicalCertificateNumber))
            throw new ArgumentException("Medical certificate is required for Practical Instructor");
        
        Specializations.Add(new InstructorSpecialization(this, InstructorType.PracticalInstructor));
        DrivingLicenseNumber = drivingLicenseNumber;
        MedicalCertificateNumber = medicalCertificateNumber;
    }
    
    public void AddTheoreticalSpecialization()
    {
        if (HasType(InstructorType.TheoreticalInstructor))
            throw new InvalidOperationException("Instructor already has Theoretical specialization");

        Specializations.Add(new InstructorSpecialization(this, InstructorType.TheoreticalInstructor));
    }
    
    public void RemoveSpecialization(InstructorType type)
    {
        if (Specializations.Count == 1)
        { 
            throw new InvalidOperationException("Instructor must have at least one specialization");
        }

        var specialization = Specializations.FirstOrDefault(x => x.Type == type);
        if (specialization == null)
            throw new InvalidOperationException($"Instructor is not {type}");

        Specializations.Remove(specialization);
        switch (type)
        {
            case InstructorType.PracticalInstructor:
                _drivingLicenseNumber = null;
                _medicalCertificateNumber = null;
                break;
            case InstructorType.TheoreticalInstructor:
                Certifications.Clear();
                break;
        }
    }
    
    public Certification AddCertification(string description)
    {
        EnsureType(InstructorType.TheoreticalInstructor);

        var certification = new Certification(description)
        {
            Instructor = this
        };
        Certifications.Add(certification);
        return certification;
    }
    
    public void RemoveCertification(int certificationId)
    {
        EnsureType(InstructorType.TheoreticalInstructor);

        var cert = Certifications.FirstOrDefault(c => c.Id == certificationId);
        if (cert == null)
            throw new InvalidOperationException("Certification not found");

        cert.Instructor = null;
        cert.InstructorId = null;
        Certifications.Remove(cert);
    }
}