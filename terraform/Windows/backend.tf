terraform {
  backend "consul" {
    address = "consul.cicd.canadalife.bz"
    scheme  = "https"
    path    = "terraform/state/ecs/CLDPLY/Azure-Virtual-Desktop-azure-vdi-az-vdi-chinww-PRD"
    gzip    = true
  }
}