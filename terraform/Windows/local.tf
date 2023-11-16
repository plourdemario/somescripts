locals {
  domain-join-user-key = format("%s%s",var.vm-domain,".user")
  domain-join-pass-key = format("%s%s",var.vm-domain,".password")
}