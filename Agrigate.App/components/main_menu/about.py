from PySide6.QtCore import Qt
from PySide6.QtWidgets import QDialog, QVBoxLayout, QLabel, QPushButton

class About(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)

        self.setWindowTitle("About")
        self.setFixedSize(350, 150)

        layout = QVBoxLayout()
        layout.setAlignment(Qt.AlignmentFlag.AlignCenter)

        title = QLabel("Application: Agrigate", self)
        title.setAlignment(Qt.AlignmentFlag.AlignLeft)

        version = QLabel("Version: 0.1.0", self)
        version.setAlignment(Qt.AlignmentFlag.AlignLeft)

        developed_by = QLabel("Developed By: Kevin Williams", self)
        developed_by.setAlignment(Qt.AlignmentFlag.AlignLeft)

        ok = QPushButton("OK", self)
        ok.clicked.connect(self.close)

        layout.addWidget(title)
        layout.addWidget(version)
        layout.addWidget(developed_by)
        layout.addWidget(ok)

        self.setLayout(layout)