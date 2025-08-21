# Use Case: Stop line

- <u>Primary actor</u>: Line stopper
- <u>Secondary actor</u>: Operator
- <u>Preconditions</u>: None
- <u>Main success scenario</u>:
  1. LineStopper: requests to stop the line
  2. System: stops the line and shows a stop line
- <u>Success guarantee</u>: The line is stopped
- <u>Exceptions</u>:
  - 1.1 The line is already stopped
    - 1.1.1 System: releases current stop
    - 1.1.2 Goto step 2
  - 1.2 The machine is already stopped
    - 1.2.1 Goto step 2
