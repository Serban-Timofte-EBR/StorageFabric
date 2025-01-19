using System.Diagnostics;
using System.IO;

namespace StorageFabricBackend.Services {
    public class TerraformService {
        public string RunTerraformScript(string resourceGroupName, string location, int vmCount) {
            string scriptPath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "Terraform");

            if (!Directory.Exists(scriptPath)) {
                throw new DirectoryNotFoundException($"The directory '{scriptPath}' does not exist.");
            }

            // Generate tfvars file
            string tfvarsPath = Path.Combine(scriptPath, "terraform.tfvars");
            File.WriteAllText(tfvarsPath, $@"
resource_group_name = ""{resourceGroupName}""
location = ""{location}""
vm_count = {vmCount}
");

            // Start the Terraform process
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "terraform",
                    Arguments = "apply -auto-approve",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = scriptPath
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0) {
                throw new Exception($"Terraform failed with exit code {process.ExitCode}. Error: {error}");
            }

            return output;
        }

        public string DestroyTerraformResources() {
            string scriptPath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "Terraform");

            if (!Directory.Exists(scriptPath)) {
                throw new DirectoryNotFoundException($"The directory '{scriptPath}' does not exist.");
            }

            // Start the Terraform destroy process
            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "terraform",
                    Arguments = "destroy -auto-approve",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = scriptPath
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0) {
                throw new Exception($"Terraform destroy failed with exit code {process.ExitCode}. Error: {error}");
            }

            return output;
        }
    }
}