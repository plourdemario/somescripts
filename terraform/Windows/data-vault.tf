data "vault_generic_secret" "domain-join-credential" {
  path = "secret/ecs/platform/vdi_domain_join"
}

data "vault_generic_secret" "vm-build-id" {
  path = "secret/ecs/platform/vdi_default_build_id"
}

data "vault_generic_secret" "vault-spoke-spn" {
  path = format(
    "%s/%s",
    "secret/ecs/platform",
    var.spoke-spn
  )
}

data "vault_generic_secret" "vault-south-hub-spn" {
  path = "secret/ecs/platform/GLC-CA-SSP01-SPN"
}

data "vault_generic_secret" "vault-north-hub-spn" {
  path = "secret/ecs/platform/CL-CT-ECS_NorthHub-PRD-SP"
}

data "vault_generic_secret" "vault-aad-readonly" {
  path = "secret/ecs/platform/CL-CT-AAD-READONLY-SPN"
}