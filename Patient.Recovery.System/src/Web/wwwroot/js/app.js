// src/Web/PatientRecovery.Web/wwwroot/js/app.js

// API Configuration
const API_BASE_URL = "http://localhost:5000/api"; // API Gateway URL

// Global variables
let patients = [];
let monitoringRecords = [];
let vitalsChart = null;
let diagnosis = [];
let rehabilitationPlans = [];

async function loadPatientMonitoring(params) {}

async function initializePage() {
    patients = await loadPatients(); // <- wait to load actual data
    loadPatientsTable();
}

// Initialize application
document.addEventListener("DOMContentLoaded", function () {
    initializeApp();
});

async function initializeApp() {
    try {
        showLoading(true);
        await loadPatients();
        await loadDashboard();
        showLoading(false);
    } catch (error) {
        console.error("Error initializing app:", error);
        showToast("Error loading application data", "error");
        showLoading(false);
    }
}

// Navigation Functions
function showSection(sectionName, element) {
    // Hide all sections
    const sections = document.querySelectorAll(".content-section");
    sections.forEach((section) => (section.style.display = "none"));

    // Show selected section
    document.getElementById(`${sectionName}-section`).style.display = "block";

    // Update navigation
    const navLinks = document.querySelectorAll(".nav-link");
    navLinks.forEach((link) => link.classList.remove("active"));

    if (element) {
        element.classList.add("active");
    }

    // Load section-specific data
    switch (sectionName) {
        case "patients":
            loadPatientsTable();
            loadDashboard();
            break;
        case "monitoring":
            loadMonitoringSection();
            break;
        case "diagnosis":
            loadDiagnosisSection();
            break;
        case "rehabilitation":
            loadRehabilitationSection();
            break;
    }
}
// showSection();

// API Functions
async function apiRequest(endpoint, method = "GET", data = null) {
    const config = {
        method,
        headers: {
            "Content-Type": "application/json",
        },
    };

    if (data) {
        config.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
        const result = await response.json();

        if (!response.ok) {
            throw new Error(result.message || "API request failed");
        }

        return result;
    } catch (error) {
        console.error("API Error:", error);
        throw error;
    }
}

async function countPatients() {
    return patients.length;
}

async function loadPatients() {
    try {
        const response = await apiRequest("/Patients");
        // console.log("API response:", response);

        // Assuming response is already the array
        const patients = response || [];

        updatePatientSelects();
        return patients;
    } catch (error) {
        console.error("Error loading patients:", error);
        showToast("Error loading patients", "error");
        return [];
    }
}

async function loadDiagnosis() {
    try {
        const response = await apiRequest("/Diagnosis");
        // console.log("API response:", response);

        // Assuming response is already the array
        const diagnosis = response || [];

        return diagnosis;
    } catch (error) {
        console.error("Error loading diagnosis:", error);
        showToast("Error loading diagnosis", "error");
        return [];
    }
}

function updatePatientSelects() {
    const selects = document.querySelectorAll(
        "#monitoring-patient-select, #monitoringPatientId"
    );

    selects.forEach((select) => {
        select.innerHTML = '<option value="">Choose a patient...</option>';
        patients.forEach((patient) => {
            const option = document.createElement("option");
            option.value = patient.id;
            option.textContent = `${patient.firstName} ${patient.lastName}`;
            select.appendChild(option);
        });
    });
}

async function loadPatientsTable() {
    patients = await loadPatients();
    const tbody = document.querySelector("#patients-table tbody");
    tbody.innerHTML = "";
    // console.log(patients);

    patients.forEach((patient) => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${patient.id}</td>
            <td>${patient.firstName} ${patient.lastName}</td>
            <td>${formatDate(patient.dateOfBirth)}</td>
            <td>${patient.gender}</td>
            <td>${patient.email || "-"}</td>
            <td>${patient.phoneNumber || "-"}</td>
            <td>${patient.address || "-"}</td>
            <td>
                <button class="btn btn-sm btn-primary" onclick="showEditPatientModal(${
                    patient.id
                })">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn btn-sm btn-danger" onclick="deletePatient(${
                    patient.id
                })">
                    <i class="fas fa-trash"></i>
                </button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

function showAddPatientModal() {
    const modal = new bootstrap.Modal(
        document.getElementById("addPatientModal")
    );
    modal.show();
}

function hidePatientModal() {
    // Close modal and reset form
    const modal = bootstrap.Modal.getInstance(
        document.getElementById("addPatientModal")
    );
    modal.hide();
    document.getElementById("addPatientForm").reset();
}

async function addPatient() {
    try {
        const formData = {
            firstName: document.getElementById("firstName").value,
            lastName: document.getElementById("lastName").value,
            dateOfBirth: document.getElementById("dateOfBirth").value,
            gender: document.getElementById("gender").value,
            email: document.getElementById("email").value,
            phoneNumber: document.getElementById("phoneNumber").value,
            address: document.getElementById("address").value,
        };

        showLoading(true);
        const response = await apiRequest("/Patients", "POST", formData);

        showToast("Patient added successfully", "success");

        patients = await loadPatients();
        loadPatientsTable();
        hidePatientModal();
    } catch (error) {
        console.error("Error adding patient:", error);
        showToast("Error adding patient", "error");
    } finally {
        showLoading(false);
    }
}

async function deletePatient(patientId) {
    if (!confirm("Are you sure you want to delete this patient?")) {
        return;
    }

    try {
        showLoading(true);
        const response = await fetch(`${API_BASE_URL}/Patients/${patientId}`, {
            method: "DELETE",
        });

        if (response.status == 204) {
            showToast("Patient deleted successfully", "success");
            await loadPatients();
            loadPatientsTable();
        } else {
            showToast("Failed to delete patient", "error");
        }
    } catch (error) {
        console.error("Error deleting patient:", error);
        showToast("Error deleting patient", "error");
    } finally {
        showLoading(false);
    }
}

async function deleteDiagnos(patientId) {
    if (!confirm("Are you sure you want to delete this diagnosis?")) {
        return;
    }

    try {
        showLoading(true);
        const response = await fetch(`${API_BASE_URL}/Diagnosis/${patientId}`, {
            method: "DELETE",
        });

        if (response.status == 204) {
            showToast("Diagnos deleted successfully", "success");
            await loadDiagnosisSection();
            await loadPatients();
        } else {
            showToast("Failed to delete diagnosis", "error");
        }
    } catch (error) {
        console.error("Error deleting diagnosis:", error);
        showToast("Error deleting diagnosis", "error");
    } finally {
        showLoading(false);
    }
}

async function showEditPatientModal(patientId) {
    try {
        // Get patient data from your patients array or make an API call
        const patient = patients.find((p) => p.id === patientId);

        if (!patient) {
            showToast("Patient not found", "error");
            return;
        }

        // Populate the form fields with existing patient data
        document.getElementById("firstName").value = patient.firstName || "";
        document.getElementById("lastName").value = patient.lastName || "";
        document.getElementById("dateOfBirth").value =
            patient.dateOfBirth || "";
        document.getElementById("gender").value = patient.gender || "";
        document.getElementById("email").value = patient.email || "";
        document.getElementById("phoneNumber").value =
            patient.phoneNumber || "";
        document.getElementById("address").value = patient.address || "";

        // Change the modal title and button text
        document.querySelector("#addPatientModal .modal-title").textContent =
            "Edit Patient";

        // Store the patient ID for later use
        document
            .getElementById("addPatientModal")
            .setAttribute("data-patient-id", patientId);

        // Change the button onclick to call editPatient instead of addPatient
        const submitButton = document.querySelector(
            "#addPatientModal .btn-primary"
        );
        submitButton.textContent = "Update Patient";
        submitButton.setAttribute("onclick", `editPatient(${patientId})`);

        showAddPatientModal();
        // await loadPatients(); // Reload patients data
        // loadPatientsTable(); // Refresh UI table
    } catch (error) {
        console.error("Error loading patient data:", error);
        showToast("Error loading patient data", "error");
    }
}

async function editPatient(patientId) {
    try {
        const formData = {
            firstName: document.getElementById("firstName").value,
            lastName: document.getElementById("lastName").value,
            dateOfBirth: document.getElementById("dateOfBirth").value,
            gender: document.getElementById("gender").value,
            email: document.getElementById("email").value,
            phoneNumber: document.getElementById("phoneNumber").value,
            address: document.getElementById("address").value,
        };

        showLoading(true);

        const response = await apiRequest(
            `/Patients/${patientId}`,
            "PUT",
            formData
        );

        showToast("Patient edited successfully", "success");

        await loadPatients(); // Reload patients data
        loadPatientsTable(); // Refresh UI table
        hidePatientModal();
        document.querySelector("#addPatientModal .modal-title").textContent =
            "Add New Patient";

        // Change the button onclick to call editPatient instead of addPatient
        const submitButton = document.querySelector(
            "#addPatientModal .btn-primary"
        );
        submitButton.textContent = "Add Patient";
        submitButton.setAttribute("onclick", `addPatient()`);
    } catch (error) {
        console.error("Error updating patient:", error);
        showToast("Error updating patient", "error");
    } finally {
        showLoading(false);
    }
}

function viewMonitoringDetails(patientId) {
    fetch(`${API_BASE_URL}/Monitoring/${patientId}`)
        .then((response) => {
            if (!response.ok) throw new Error("Monitoring data not found");
            return response.json();
        })
        .then((data) => {
            console.log("API Response:", data);

            let html = `
                <h5>Monitoring Details</h5>
                <ul>
                    <li>Time: ${new Date(data.recordedAt).toLocaleString()}</li>
                    <li>Temperature: ${data.temperature}°C</li>
                    <li>Blood Pressure: ${data.bloodPressureSystolic}/${
                data.bloodPressureDiastolic
            }</li>
                    <li>Heart Rate: ${data.heartRate} bpm</li>
                    <li>Weight: ${data.weight} kg</li>
                    <li>Location: ${data.location}</li>
                    <li>Symptoms: ${data.symptoms || "N/A"}</li>
                    <li>Notes: ${data.notes || "N/A"}</li>
                </ul>
            `;

            document.getElementById("monitoringDetailsContainer").innerHTML =
                html;

            const modal = new bootstrap.Modal(
                document.getElementById("monitoringModal")
            );
            modal.show();
        })
        .catch((error) => {
            alert("Error: " + error.message);
        });
}

// Monitoring Functions
async function loadMonitoringSection() {
    await loadPatients();
}

function showAddMonitoringModal() {
    const modal = new bootstrap.Modal(
        document.getElementById("addMonitoringModal")
    );
    modal.show();
}

async function addMonitoringRecord() {
    try {
        const formData = {
            patientId: parseInt(
                document.getElementById("monitoringPatientId").value
            ),
            temperature:
                parseFloat(document.getElementById("temperature").value) ||
                null,
            bloodPressureSystolic:
                parseInt(
                    document.getElementById("bloodPressureSystolic").value
                ) || null,
            bloodPressureDiastolic:
                parseInt(
                    document.getElementById("bloodPressureDiastolic").value
                ) || null,
            heartRate:
                parseInt(document.getElementById("heartRate").value) || null,
            weight: parseFloat(document.getElementById("weight").value) || null,
            symptoms: document.getElementById("symptoms").value,
            notes: document.getElementById("notes").value,
            location: document.getElementById("location").value,
            recordedBy: document.getElementById("recordedBy").value,
        };

        showLoading(true);
        const response = await apiRequest(
            `/Monitoring/${formData.patientId}`,
            "POST",
            formData
        );

        showToast("Monitoring record added successfully", "success");
        await loadPatientMonitoring();

        // Close modal and reset form
        const modal = bootstrap.Modal.getInstance(
            document.getElementById("addMonitoringModal")
        );
        modal.hide();
        document.getElementById("addMonitoringForm").reset();
    } catch (error) {
        console.error("Error adding monitoring record:", error);
        showToast("Error adding monitoring record", "error");
    } finally {
        showLoading(false);
    }
}

async function loadPatientMonitoring() {
    const patientId = document.getElementById(
        "monitoring-patient-select"
    ).value;
    const timeframe = document.getElementById("monitoring-timeframe").value;

    if (!patientId || patientId === "") {
        // console.log(patientId);
        document.querySelector("#monitoring-table tbody").innerHTML = "";

        console.log(vitalsChart);
        if (vitalsChart) {
            vitalsChart.destroy();
            vitalsChart = null;
        }
        return;
    }

    try {
        showLoading(true);

        const response = await fetch(
            `${API_BASE_URL}/Monitoring/patient/${patientId}/recent?hours=${timeframe}`,
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    // Add Authorization header here if needed
                },
            }
        );

        if (!response.ok) {
            console.error("Request failed:", response.statusText);
            return;
        }

        const data = await response.json();
        // console.log(data);

        monitoringRecords = data || [];
        // console.log(monitoringRecords)
        updateMonitoringTable();
        updateVitalsChart();

        // Check for alarms
        await checkPatientAlarms(patientId);
    } catch (error) {
        console.error("Error loading patient monitoring:", error);
        showToast("Error loading monitoring data", "error");
    } finally {
        showLoading(false);
    }
}

function updateMonitoringTable() {
    const tbody = document.querySelector("#monitoring-table tbody");
    tbody.innerHTML = "";

    monitoringRecords.forEach((record) => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${formatDateTime(record.recordedAt)}</td>
            <td>${record.temperature ? record.temperature.toFixed(1) : "-"}</td>
            <td>${
                record.bloodPressureSystolic && record.bloodPressureDiastolic
                    ? `${record.bloodPressureSystolic}/${record.bloodPressureDiastolic}`
                    : "-"
            }</td>
            <td>${record.heartRate || "-"}</td>
            <td>${record.weight ? record.weight.toFixed(1) : "-"}</td>
            <td>${record.location || "-"}</td>
            <td>${record.recordedBy || "-"}</td>
            <td>
                <button class="btn btn-sm btn-info" onclick="viewMonitoringDetails(${
                    record.id
                })">
                    <i class="fas fa-eye"></i>
                </button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

function updateVitalsChart() {
    const ctx = document.getElementById("vitals-chart").getContext("2d");

    if (vitalsChart) {
        vitalsChart.destroy();
    }

    const labels = monitoringRecords
        .map((record) => formatTime(record.recordedAt))
        .reverse();
    const temperatureData = monitoringRecords
        .map((record) => record.temperature)
        .reverse();
    const heartRateData = monitoringRecords
        .map((record) => record.heartRate)
        .reverse();
    const systolicData = monitoringRecords
        .map((record) => record.bloodPressureSystolic)
        .reverse();

    vitalsChart = new Chart(ctx, {
        type: "line",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "Temperature (°C)",
                    data: temperatureData,
                    borderColor: "rgb(255, 99, 132)",
                    backgroundColor: "rgba(255, 99, 132, 0.1)",
                    yAxisID: "y",
                },
                {
                    label: "Heart Rate (bpm)",
                    data: heartRateData,
                    borderColor: "rgb(54, 162, 235)",
                    backgroundColor: "rgba(54, 162, 235, 0.1)",
                    yAxisID: "y1",
                },
                {
                    label: "Systolic BP",
                    data: systolicData,
                    borderColor: "rgb(255, 205, 86)",
                    backgroundColor: "rgba(255, 205, 86, 0.1)",
                    yAxisID: "y1",
                },
            ],
        },
        options: {
            responsive: true,
            interaction: {
                mode: "index",
                intersect: false,
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: "Time",
                    },
                },
                y: {
                    type: "linear",
                    display: true,
                    position: "left",
                    title: {
                        display: true,
                        text: "Temperature (°C)",
                    },
                    min: 35,
                    max: 42,
                },
                y1: {
                    type: "linear",
                    display: true,
                    position: "right",
                    title: {
                        display: true,
                        text: "Heart Rate (bpm) / Blood Pressure",
                    },
                    grid: {
                        drawOnChartArea: false,
                    },
                    min: 40,
                    max: 200,
                },
            },
        },
    });
}

async function checkPatientAlarms(patientId) {
    try {
        const response = await apiRequest(
            `/Monitoring/patient/${patientId}/check-alarms`
        );

        if (response.success && response.data) {
            showAlert("Health Alert", response.message, "warning");
        }
    } catch (error) {
        console.error("Error checking alarms:", error);
    }
}

// Dashboard Functions
async function loadDashboard() {
    try {
        const patients = await loadPatients();
        // Update statistics
        document.getElementById("total-patients").textContent = patients.length;

        // Load recent monitoring records for dashboard
        const recentResponse = await apiRequest("/Monitoring?hours=24");

        // console.log(recentResponse);
        if (Array.isArray(recentResponse) && recentResponse.length > 0) {
            // const recentRecords = records.data || [];
            updateDashboardMonitoringTable(recentResponse);
        }

        const active_monitorings = await apiRequest("/Monitoring");
        const pending_diagnosis = await apiRequest("/Monitoring");


        // Update other stats (mock data for now)
        document.getElementById("active-monitoring").textContent = active_monitorings.length;
        document.getElementById("pending-diagnosis").textContent = Math.floor(
            Math.floor(diagnosis.length)
        );
        document.getElementById("rehabilitation-plans").textContent =
            Math.floor(rehabilitationPlans.length);
    } catch (error) {
        console.error("Error loading dashboard:", error);
    }
}

function updateDashboardMonitoringTable(records) {
    const tbody = document.querySelector("#recent-monitoring-table tbody");
    tbody.innerHTML = "";

    // Show only last 5 records
    const recentRecords = records.slice(records.length - 5, records.length);

    recentRecords.reverse();
    // console.log(records);
    recentRecords.forEach((record) => {
        const patient = patients.find((p) => p.id === record.patientId);
        const patientName = patient
            ? `${patient.firstName} ${patient.lastName}`
            : "Unknown";

        // Determine status based on vitals
        let status = "Normal";
        let statusClass = "text-success";

        if (
            record.temperature &&
            (record.temperature > 38.5 || record.temperature < 35.0)
        ) {
            status = "Alert";
            statusClass = "text-danger";
        } else if (
            record.heartRate &&
            (record.heartRate > 120 || record.heartRate < 50)
        ) {
            status = "Warning";
            statusClass = "text-warning";
        }

        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${patientName}</td>
            <td>${
                record.temperature ? record.temperature.toFixed(1) + "°C" : "-"
            }</td>
            <td>${
                record.bloodPressureSystolic && record.bloodPressureDiastolic
                    ? `${record.bloodPressureSystolic}/${record.bloodPressureDiastolic}`
                    : "-"
            }</td>
            <td>${record.heartRate ? record.heartRate + " bpm" : "-"}</td>
            <td>${formatTime(record.recordedAt)}</td>
            <td><span class="${statusClass}">${status}</span></td>
        `;
        tbody.appendChild(row);
    });
}

// Diagnosis Functions (placeholder)
async function loadDiagnosisSection() {
    diagnosis = await loadDiagnosis();

    diagnosis.forEach((d) => {
        console.log(d);
    });
    const tbody = document.querySelector("#diagnosis-table tbody");
    tbody.innerHTML = "";
    // console.log(patients);

    diagnosis.forEach((diagnos) => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${diagnos.patientId}</td>
            <td>${diagnos.symptoms || "-"}</td>
            <td>${diagnos.diagnosisName || "-"}</td>
            <td>${diagnos.treatment || "-"}</td>
            <td>${diagnos.status || "-"}</td>
            <td>${diagnos.severity || "-"}</td>
            <td>${diagnos.notes || "-"}</td>
            <td>
            ${
                diagnos.diagnosisDate
                    ? (() => {
                          const d = new Date(diagnos.diagnosisDate);
                          const year = d.getFullYear();
                          const month = String(d.getMonth() + 1).padStart(
                              2,
                              "0"
                          );
                          const day = String(d.getDate()).padStart(2, "0");
                          return `${year}/${month}/${day}`;
                      })()
                    : "-"
            }
            </td>
            <td>${diagnos.doctorName || "-"}</td>
            <td>
                <button class="btn btn-sm btn-primary" onclick="showUpdateDiagnosisModal(${
                    diagnos.patientId
                })">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn btn-sm btn-danger" onclick="deleteDiagnos(${
                    diagnos.id
                })">
                    <i class="fas fa-trash"></i>
                </button>
            </td>`;
        tbody.appendChild(row);
    });
    // Placeholder for diagnosis functionality
    console.log("Loading diagnosis section...");
}

function showAddDiagnosisModal() {
    const modal = new bootstrap.Modal(
        document.getElementById("addDiagnosisModal")
    );
    modal.show();
    // showToast("Diagnosis feature coming soon!", "info");
}

function hideDiagnosisModal() {
    // Close modal and reset form
    const modal = bootstrap.Modal.getInstance(
        document.getElementById("addDiagnosisModal")
    );
    modal.hide();
    document.getElementById("addDiagnosisForm").reset();
}

async function addDiagnosis() {
    try {
        const patientId = document.getElementById("patientId").value;
        const formData = {
            symptoms: document.getElementById("symptoms").value,
            diagnosisName: document.getElementById("diagnosisName").value,
            treatment: document.getElementById("treatment").value,
            status: document.getElementById("status").value,
            severity: document.getElementById("severity").value,
            notes: document.getElementById("notes").value,
            doctorName: document.getElementById("doctorName").value,
        };

        showLoading(true);
        const response = await apiRequest(
            `/Diagnosis/${patientId}`,
            "POST",
            formData
        );

        showToast("Diagnosis added successfully", "success");

        diagnosis = await loadDiagnosis();
        loadDiagnosisSection();
        hideDiagnosisModal();
    } catch (error) {
        console.error("Error adding diagnosis:", error);
        showToast("Error adding diagnosis", "error");
    } finally {
        showLoading(false);
    }
}

async function editDiagnosis(patientId) {
    try {
        const formData = {
            symptoms: document.getElementById("symptoms").value,
            diagnosisName: document.getElementById("diagnosisName").value,
            treatment: document.getElementById("treatment").value,
            status: document.getElementById("status").value,
            severity: document.getElementById("severity").value,
            notes: document.getElementById("notes").value,
            doctorName: document.getElementById("doctorName").value,
        };

        showLoading(true);

        const response = await apiRequest(
            `/Diagnosis/${patientId}`,
            "PUT",
            formData
        );
        console.log(response.success);

        showToast("Diagnosis updated successfully", "success");

        diagnosis = await loadDiagnosis();
        loadDiagnosisSection();
        hideDiagnosisModal();
    } catch (error) {
        console.error("Error updating diagnosis:", error);
        showToast("Error updating diagnosis", "error");
    } finally {
        showLoading(false);
    }
}

function showEditDiagnosisModal(diagnosis) {
    try {
        if (!diagnosis || !diagnosis.patientId) {
            showToast("Invalid diagnosis data", "error");
            return;
        }

        // Fill the form with diagnosis data
        document.getElementById("symptoms").value = diagnosis.symptoms || "";
        document.getElementById("diagnosisName").value =
            diagnosis.diagnosisName || "";
        document.getElementById("treatment").value = diagnosis.treatment || "";
        document.getElementById("status").value = diagnosis.status || "";
        document.getElementById("severity").value = diagnosis.severity || "";
        document.getElementById("notes").value = diagnosis.notes || "";
        document.getElementById("doctorName").value =
            diagnosis.doctorName || "";

        // Optional: Set hidden fields (if needed)
        document.getElementById("diagnosisId").value = diagnosis.id;
        document.getElementById("patientId").value = diagnosis.patientId;

        // Change modal title and button
        document.querySelector("#addDiagnosisModal .modal-title").textContent =
            "Edit Diagnosis";

        const submitButton = document.querySelector(
            "#addDiagnosisModal .btn-primary"
        );
        submitButton.textContent = "Update Diagnosis";
        submitButton.setAttribute(
            "onclick",
            `editDiagnosis(${diagnosis.id}, ${diagnosis.patientId})`
        );

        // Show the modal
        const modal = new bootstrap.Modal(
            document.getElementById("addDiagnosisModal")
        );
        modal.show();
    } catch (error) {
        console.error("Error showing edit modal:", error);
        showToast("Error loading diagnosis data", "error");
    }
}

function showUpdateDiagnosisModal() {
    showToast("Update Diagnosis feature coming soon!", "info");
}

// Rehabilitation Functions (placeholder)
async function loadRehabilitationSection() {
    // if (!patientId) {
    //     console.warn("No patient ID provided for rehabilitation data.");
    //     return;
    // }

    try {
        showLoading(true); // Optional: show loading spinner

        const response = await fetch(`${API_BASE_URL}/RehabilitationPlans`);
        if (!response.ok)
            throw new Error("Failed to load rehabilitation plans");

        const plans = await response.json();

        const tableBody = document.querySelector("#rehabilitation-table tbody");
        tableBody.innerHTML = ""; // Clear any existing rows

        if (plans.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="9" class="text-center">No rehabilitation plans found.</td></tr>`;
            return;
        }

        plans.forEach((plan) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${plan.patientId}</td>
                <td>${plan.planName}</td>
                <td>${new Date(plan.startDate).toLocaleDateString()}</td>
                <td>${new Date(plan.endDate).toLocaleDateString()}</td>
                <td>${plan.progress}%</td>
                <td>${plan.status}</td>
                <td>${plan.therapistName}</td>
                <td>${new Date(plan.createdAt).toLocaleDateString()}</td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="showEditRehabilitationModal(${
                        plan.id
                    })">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="deleteRehabilitationPlan(${
                        plan.id
                    })">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error("Error loading rehabilitation plans:", error);
        showToast("Error loading rehabilitation plans", "error");
    } finally {
        showLoading(false); // Optional: hide loading spinner
    }
}

async function addRehabilitationPlan() {
    try {
        const patientId = document.getElementById("rehabPatientId").value;

        const formData = {
            planName: document.getElementById("planName").value.trim(),
            startDate: document.getElementById("startDate").value,
            endDate: document.getElementById("endDate").value,
            progress: parseInt(document.getElementById("progress").value) || 0,
            status: document.getElementById("rehabstatus").value,
            notes: document.getElementById("notes").value,
            therapistName: document
                .getElementById("therapistName")
                .value.trim(),
            createdDate: new Date().toISOString(), // if needed by API
        };

        showLoading(true);
        const response = await apiRequest(
            `/RehabilitationPlans/${patientId}`,
            "POST",
            formData
        );

        showToast("Rehabilitation added successfully", "success");
        
        await loadRehabilitationSection();
        hideRehabilitationModal();
        document.getElementById("addRehabilitationForm").reset();
    } catch (error) {
        console.error("Error adding rehabilitation:", error);
        showToast("Error adding rehabilitation", "error");
    } finally {
        showLoading(false);
    }
}

function showAddRehabilitationModal() {
    const modal = new bootstrap.Modal(
        document.getElementById("addRehabilitationModal")
    );
    modal.show();
}

function hideRehabilitationModal() {
    // Close modal and reset form
    const modal = bootstrap.Modal.getInstance(
        document.getElementById("addRehabilitationModal")
    );
    modal.hide();
    document.getElementById("addRehabilitationForm").reset();
}

// async function showEditRehabilitationModal(planId) {
//     try {
//         const response = await fetch(`${API_BASE_URL}/Rehabilitation/${planId}`);
//         if (!response.ok) throw new Error("Failed to fetch rehabilitation plan");

//         const plan = await response.json();

//         // Fill form fields
//         document.getElementById("rehabPatientId").value = plan.patientId;
//         document.getElementById("planName").value = plan.planName;
//         document.getElementById("startDate").value = plan.startDate.split("T")[0];
//         document.getElementById("endDate").value = plan.endDate.split("T")[0];
//         document.getElementById("progress").value = plan.progress;
//         document.getElementById("status").value = plan.status;
//         document.getElementById("therapistName").value = plan.therapistName;
//         document.getElementById("notes").value = plan.notes;

//         // Store plan ID in a hidden field or dataset
//         document.getElementById("rehabilitationModal").setAttribute("data-plan-id", planId);

//         // Set modal title and button
//         document.querySelector("#rehabilitationModal .modal-title").textContent = "Edit Rehabilitation Plan";
//         const submitBtn = document.querySelector("#rehabilitationModal .btn-primary");
//         submitBtn.textContent = "Update";
//         submitBtn.onclick = () => updateRehabilitationPlan(planId);

//         const modal = new bootstrap.Modal(document.getElementById("rehabilitationModal"));
//         modal.show();

//     } catch (error) {
//         console.error("Error loading plan for editing:", error);
//         showToast("Failed to load rehabilitation plan", "error");
//     }
// }

function showEditRehabilitationModal() {
    showToast("Rehabilitation Update feature coming soon!", "info");
}

async function deleteRehabilitationPlan(planId) {
    if (!confirm("Are you sure you want to delete this rehabilitation plan?"))
        return;

    try {
        showLoading(true);
        const response = await fetch(
            `${API_BASE_URL}/RehabilitationPlans/${planId}`,
            {
                method: "DELETE",
            }
        );

        if (!response.ok) throw new Error("Delete failed");

        showToast("Rehabilitation plan deleted", "success");
        await loadRehabilitationSection();
        const patientId =
            document.getElementById("rehabPatientId")?.value || planId; // Adjust if needed
    } catch (error) {
        console.error("Delete error:", error);
        showToast("Failed to delete rehabilitation plan", "error");
    } finally {
        showLoading(false);
    }
}

// Utility Functions
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}

function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString();
}

function formatTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
}

function showLoading(show) {
    const spinner = document.getElementById("loading-spinner");
    spinner.style.display = show ? "block" : "none";
}

function showToast(message, type = "info") {
    const toastContainer = document.getElementById("toast-container");
    const toastId = "toast-" + Date.now();

    const toastColors = {
        success: "text-bg-success",
        error: "text-bg-danger",
        warning: "text-bg-warning",
        info: "text-bg-info",
    };

    const toastHtml = `
        <div id="${toastId}" class="toast ${toastColors[type]}" role="alert">
            <div class="toast-header">
                <strong class="me-auto">System Notification</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;

    toastContainer.insertAdjacentHTML("beforeend", toastHtml);

    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement);
    toast.show();

    // Remove toast from DOM after it's hidden
    toastElement.addEventListener("hidden.bs.toast", () => {
        toastElement.remove();
    });
}

function showAlert(title, message, type = "info") {
    const alertsContainer = document.getElementById("alerts-container");

    const alertColors = {
        success: "alert-success",
        error: "alert-danger",
        warning: "alert-warning",
        info: "alert-info",
    };

    const alertHtml = `
        <div class="alert ${alertColors[type]} alert-dismissible fade show" role="alert">
            <strong>${title}:</strong> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    alertsContainer.insertAdjacentHTML("beforeend", alertHtml);
}

// Event Listeners
document
    .getElementById("monitoring-patient-select")
    .addEventListener("change", loadPatientMonitoring);
document
    .getElementById("monitoring-timeframe")
    .addEventListener("change", loadPatientMonitoring);

// Auto-refresh dashboard every 30 seconds
setInterval(async () => {
    if (document.getElementById("dashboard-section").style.display !== "none") {
        await loadDashboard();
    }
}, 30000);
