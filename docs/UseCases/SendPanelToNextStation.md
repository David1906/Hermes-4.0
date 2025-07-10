# Use Case: Send panel to the next station

- <u>Primary actor</u>: Machine
- <u>Preconditions</u>: Machine is not stopped, valid logfile extension, panel exists
- <u>Main success scenario</u>:
    1. Machine requests to send the panel to the next station
    2. The system requests the logfile and panel
    3. Machine provides the logfile and panel
    4. The system sends the panel to the next station
    5. The system shows a success message
- <u>Success guarantee</u>: Panel's group is in the next station
- <u>Exceptions</u>:
    - 4a Connection error
        - The system shows stop for EE/IT
        - End of the use case
    - 4a Sfc response times out
        - The system shows stop for EE/IT
        - End of the use case
    - 4b Unable to send the panel to the next station
        - The system shows stop for QA
        - End of the use case
