web_servers = [
  {
    share:        '\\\\wjv-web01.ehdmz.com\\stage$\\stageclientportal.extendhealth.com',
    machine:     'wjv-web01.ehdmz.com',
    app_pool:    'stageclientportal.extendhealth.com'
  },
  {
    share:        '\\\\wjv-web02.ehdmz.com\\stage$\\stageclientportal.extendhealth.com',
    machine:     'wjv-web02.ehdmz.com',
    app_pool:    'stageclientportal.extendhealth.com'
  }
]

service_servers = [
  {
    path:         'C:\Services\stage\ClientManagement Services - Stage',
    service_name: 'ClientManagement.Services.Stage',
    machine:      'wjv-app03'
  },
]
