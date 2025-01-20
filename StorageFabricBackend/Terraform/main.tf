variable "resource_group_name" {
  description = "The name of the resource group"
  type        = string
}

variable "location" {
  description = "The Azure region where resources will be created"
  type        = string
  default     = "East US"
}

variable "vm_count" {
  description = "Number of virtual machines to create"
  type        = number
  default     = 1
}

variable "ssh_public_key_path" {
  description = "Path to the SSH public key file"
  type        = string
  default     = "~/.ssh/id_rsa.pub"
}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "storage_fabric_rg" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_virtual_network" "storage_fabric_vnet" {
  name                = "${var.resource_group_name}-vnet"
  location            = azurerm_resource_group.storage_fabric_rg.location
  resource_group_name = azurerm_resource_group.storage_fabric_rg.name
  address_space       = ["10.0.0.0/16"]
}

resource "azurerm_subnet" "storage_fabric_subnet" {
  name                 = "${var.resource_group_name}-subnet"
  resource_group_name  = azurerm_resource_group.storage_fabric_rg.name
  virtual_network_name = azurerm_virtual_network.storage_fabric_vnet.name
  address_prefixes     = ["10.0.1.0/24"]
}

resource "azurerm_linux_virtual_machine" "storage_fabric_vms" {
  count               = var.vm_count
  name                = "${var.resource_group_name}-vm-${count.index + 1}"
  resource_group_name = azurerm_resource_group.storage_fabric_rg.name
  location            = azurerm_resource_group.storage_fabric_rg.location
  size                = "Standard_B1ls"

  admin_username = "azureuser"

  disable_password_authentication = true

  admin_ssh_key {
    username   = "azureuser"
    public_key = file(var.ssh_public_key_path) 
  }

  network_interface_ids = [
    azurerm_network_interface.storage_fabric_nics[count.index].id
  ]

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "UbuntuServer"
    sku       = "18.04-LTS"
    version   = "latest"
  }
}

resource "azurerm_network_interface" "storage_fabric_nics" {
  count               = var.vm_count
  name                = "${var.resource_group_name}-nic-${count.index + 1}"
  location            = azurerm_resource_group.storage_fabric_rg.location
  resource_group_name = azurerm_resource_group.storage_fabric_rg.name

  ip_configuration {
    name                          = "${var.resource_group_name}-ipconfig-${count.index + 1}"
    subnet_id                     = azurerm_subnet.storage_fabric_subnet.id
    private_ip_address_allocation = "Dynamic"
  }
}

output "resource_group_name" {
  value = azurerm_resource_group.storage_fabric_rg.name
}

output "virtual_machine_names" {
  value = azurerm_linux_virtual_machine.storage_fabric_vms[*].name
}