scaffold-dbcontext `
	"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ContosoUniv;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" `
	Microsoft.EntityFrameworkCore.SqlServer `
	-context "ContosoUnivContextBase" `
	-outputdir "Scaffold" `
	-namespace "ContosoUniv.Data" `
	-NoPluralize `
	-force