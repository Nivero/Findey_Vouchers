# Findey_Vouchers
Findey vouchers giftcard app

# Updating database
`$dotnet ef database update -c FindeyVouchers.Domain.ApplicationDbContext --startup-project ../FindeyVouchers.Cms`

# Deployment
.net core app published and placed in /home/debian/dotnet
make sure to copy appsettings
to restart:
- sudo systemctl restart findeyvouchers-cms.service
- sudo systemctl restart findeyvouchers-website.service

Nginx reversproxy with wildcard -> hard url or portal.findey.nl
lets encrypt with certbot.
to renew:
sudo certbot -i nginx --dns-ovh --dns-ovh-credentials ./ovh-dns-credentials -d *.findey.nl
sudo certbot -i nginx --dns-ovh --dns-ovh-credentials ./ovh-dns-credentials -d portal.findey.nl