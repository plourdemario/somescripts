# All Outputs for the workload-resources module
#output "workload-outputs" {
#  value = module.workload-resources
#}

# All Outputs for the WVD Module
output "wvd-outputs" {
  value = module.wvd
  sensitive = true
}

# Run Provisioning
output "run-provisioning" {
  value = var.run-provisioning
}

# Install French
output "install_french" {
  value = var.install_french
}
