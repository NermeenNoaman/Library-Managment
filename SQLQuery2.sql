USE [HRSystem]

-- 1. موظف مبيعات جديد
INSERT INTO [dbo].[TPLEmployee]
           ([Name]
           ,[Email]
           ,[Phone]
           ,[HireDate]
           ,[JobID]
           ,[DepartmentID]
           ,[EmploymentStatus])
     VALUES
           ('Nour Elsayed'
           ,'nour.elsayed@tpl.com'
           ,'+201009988776'
           ,'2025-01-05' -- تعيين حديث
           ,11 -- Sales Executive
           ,3  -- Sales & Marketing
           ,'Active')
GO

-- 2. موظف موارد بشرية (Senior Recruiter)
INSERT INTO [dbo].[TPLEmployee]
           ([Name]
           ,[Email]
           ,[Phone]
           ,[HireDate]
           ,[JobID]
           ,[DepartmentID]
           ,[EmploymentStatus])
     VALUES
           ('Ali Hassan'
           ,'ali.hassan@tpl.com'
           ,'+201223344556'
           ,'2024-07-20'
           ,10 -- Senior Recruiter
           ,2  -- Human Resources
           ,'Active')
GO

-- 3. موظف دعم فني (Network Engineer)
INSERT INTO [dbo].[TPLEmployee]
           ([Name]
           ,[Email]
           ,[Phone]
           ,[HireDate]
           ,[JobID]
           ,[DepartmentID]
           ,[EmploymentStatus])
     VALUES
           ('Dina Samy'
           ,'dina.samy@tpl.com'
           ,'+201556789100'
           ,'2023-10-15'
           ,12 -- Network Engineer
           ,4  -- Information Technology
           ,'Active')
GO

-- 4. موظف إداري (للتجربة)
INSERT INTO [dbo].[TPLEmployee]
           ([Name]
           ,[Email]
           ,[Phone]
           ,[HireDate]
           ,[JobID]
           ,[DepartmentID]
           ,[EmploymentStatus])
     VALUES
           ('Youssef Tarek'
           ,'youssef.tarek@tpl.com'
           ,'+201112233445'
           ,'2024-05-01'
           ,13 -- Administrative Assistant
           ,6  -- General Administration
           ,'Active');

           

INSERT INTO [dbo].[LkpHRDepartments]
           ([BranchId]
           ,[NameEn] 
           ,[NameAr]
           ,[Location]
           ,[Description]
           ,[ManagerId]
           ,[CreatedBy]
           ,[IsDeleted])
     VALUES
           -- 13. Research and Development
           ( 12, N'Research and Development', N'البحث والتطوير', N'R&D Center', N'Leading innovation and future product feasibility studies.', 0, 101, 0),
           
           -- 14. Customer Service
           ( 12, N'Customer Service', N'خدمة العملاء', N'Ground Floor, West', N'Managing all client inquiries, feedback, and support tickets.', 0, 101, 0),
           
           -- 15. Quality Assurance (QA)
           ( 12, N'Quality Assurance', N'ضمان الجودة', N'Lab Wing', N'Ensuring products meet quality standards and performance requirements.', 0, 102, 0)
           
          
GO



INSERT INTO [dbo].[LkpHRDepartments]
           ([BranchId]
           ,[NameEn] 
           ,[NameAr]
           ,[Location]
           ,[Description]
           ,[ManagerId]
           ,[CreatedBy]
           ,[IsDeleted])
     VALUES
           -- 1. Logistics and Supply Chain
           ( 10, N'Logistics and Supply Chain', N'اللوجستيات وسلسلة الإمداد', N'Warehouse Block', N'Managing inventory, shipping, and supplier relationships.', 0, 101, 0),
           
           -- 2. Treasury
           ( 12, N'Treasury', N'الخزانة', N'Finance Wing', N'Handling cash flow, banking relationships, and investment management.', 0, 101, 0),
           
           -- 3. Public Relations (PR)
           ( 10, N'Public Relations (PR)', N'العلاقات العامة', N'Executive Floor', N'Managing media relations, public image, and corporate communications.', 0, 102, 0),
           
           -- 4. Facilities Management
           ( 11, N'Facilities Management', N'إدارة المرافق', N'Maintenance Office', N'Overseeing the maintenance and upkeep of all company properties.', 0, 102, 0);

GO
GO

INSERT INTO [dbo].[TPLJob]
           ([Title]
           ,[Description]
           ,[DepartmentID]
           ,[PostedDate]
           ,[Status])
     VALUES
           -- 1. Logistics Coordinator (Dept ID=17)
           ( N'Logistics Coordinator', N'Coordinates shipping schedules, tracking, and inventory levels.', 11, '2025-11-20', N'Open'),
           
           -- 2. Financial Analyst (Dept ID=18)
           ( N'Financial Analyst', N'Performs detailed financial modeling and analysis for investment strategies.', 12, '2025-10-15', N'Open'),
           
           -- 3. PR Specialist (Dept ID=19)
           ( N'Public Relations Specialist', N'Writes press releases, organizes media events, and manages social presence.', 13, '2025-11-01', N'Open'),
           
           -- 4. Maintenance Technician (Dept ID=20)
           ( N'Maintenance Technician', N'Performs routine and preventative maintenance on building systems.', 14, '2025-12-05', N'Open'),
           
           -- 5. IT Support Specialist (Dept ID=4)
           ( N'IT Support Specialist', N'Provide first-level technical support for employees, troubleshooting hardware and software issues.', 4, '2025-12-10', N'Open'),
           
           -- 6. Office Coordinator (Dept ID=6)
           ( N'Office Coordinator', N'Managing office supplies, scheduling meetings, and handling general correspondence.', 6, '2025-12-15', N'Open');

GO

GO

INSERT INTO [dbo].[TPLEmployee]
           ([Name]
           ,[Email]
           ,[Phone]
           ,[HireDate]
           ,[JobID]
           ,[DepartmentID] -- هذا هو القسم المطلوب
           ,[EmploymentStatus])
     VALUES
           ('Hassan Zaki'
           ,'hassan.zaki@tpl.com'
           ,'+201211223344'
           ,'2025-02-01' -- تاريخ تعيين افتراضي
           ,10 -- JobID (Senior Recruiter، يتبع القسم 2)
           ,2  -- DepartmentID (Human Resources)
           ,'Active')
GO