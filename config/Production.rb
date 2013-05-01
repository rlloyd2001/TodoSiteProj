web_servers = [
  {
    share:        '\\\\wjv-web01.ehdmz.com\\production$\\clientportal.extendhealth.com',
    machine:     'wjv-web01.ehdmz.com',
    app_pool:    'clientportal.extendhealth.com'
  },
  {
    share:        '\\\\wjv-web02.ehdmz.com\\production$\\clientportal.extendhealth.com',
    machine:     'wjv-web02.ehdmz.com',
    app_pool:    'clientportal.extendhealth.com'
  }
]

service_servers = [
  {
    path:         'C:\Services\production\ClientManagement Services',
    service_name: 'ClientManagement.Services',
    machine:      'wjv-app04'
  }
]
