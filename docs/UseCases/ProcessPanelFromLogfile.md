# Use Case: Process panel from logfile

- <u>Primary actor</u>: Machine
- <u>Preconditions</u>: Machine is not stopped, valid logfile extension
- <u>Main success scenario</u>:
    1. Machine requests to process panel from logfile
    2. The system requests the logfile
    3. Machine provides the logfile
    4. The system calls "Create panel from logfile" use case
    5. The system calls "Send panel to the next station" use case
- <u>Success guarantee</u>: Panel's group is in the next station
- <u>Exceptions</u>:
    - Unexpected exception
        - The system shows stop for EE
        - End of the use case
