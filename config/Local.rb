web_servers = [
  {
    path:        File.join(EXECUTING_DIR, 'Deploy', 'Web'),
    app_pool:    'LocalPool'
  }
]
service_servers = [
  {
    path:          File.join(EXECUTING_DIR, 'Deploy', 'Service'),
    service_name:  'TodoSite.Services'
  }
]
etl_servers = [
  {
    path:          File.join(EXECUTING_DIR, 'Deploy', 'Service'),
    service_name:  'TodoSite.Synchronization'
  }
]
