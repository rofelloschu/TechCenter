Public Interface IF_think
    Sub recvdata(data As Object)
    'Sub command()
    Event command(comm As Object)
    Event remember(comm As Object)
    Event save(data As Object)


End Interface
